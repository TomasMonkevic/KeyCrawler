using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace KeyCrawler.Service.Utils
{
    public class PageFetcher : IPageFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PageFetcher> _logger;

        public PageFetcher(HttpClient httpClient, ILogger<PageFetcher> logger) 
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<HtmlDocument>> GetAllPages(Uri domain)
        {
            var uris = new Queue<Uri>();
            var visitedUris = new List<Uri>();
            var pages = new List<HtmlDocument>();

            var pageCount = 0;
            _logger.LogInformation($"MAIN PAGE {domain}");
            uris.Enqueue(domain);
            while(uris.Any()) //TODO Add subpage limit
            {
                var uri = uris.Dequeue();
                visitedUris.Add(uri);
                var page = await GetPage(uri);
                if(page == null)
                {
                    continue;
                }
                pages.Add(page);
                var subPagesUris = GetUris(uri, page);

                foreach(var notVisitedPage in subPagesUris.Except(visitedUris).Except(uris)) 
                {
                    uris.Enqueue(notVisitedPage);
                }

                pageCount++;
                _logger.LogInformation($"Page count: {pageCount}");

            }
            _logger.LogInformation($"DONE FOR PAGE {domain}");
            return pages;
        }

        public IEnumerable<Uri> GetUris(Uri uri, HtmlDocument page) {
            var links = page.DocumentNode.SelectNodes("//a[@href]");
            //TODO make the slash conditional, also don't forget about link with full path and check the domain
            var subPages = links.Select(link => new Uri(uri.Scheme + "://" + uri.DnsSafeHost +"/" +link.Attributes["href"].Value));
            return subPages;
        }

        public async Task<HtmlDocument> GetPage(Uri uri) 
        {
            var response = await _httpClient.GetAsync(uri);
            if(!response.IsSuccessStatusCode) {
                return null; //CONSIDER: Maybe better to use exception.
            }
            var content = await response.Content.ReadAsStringAsync();
            var html = new HtmlDocument();
            html.LoadHtml(content); //CONSIDER: Parsing errors may occur here also.
            return html;
        } 
    }
}
