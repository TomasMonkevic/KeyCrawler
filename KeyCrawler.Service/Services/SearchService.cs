using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KeyCrawler.Domain;
using Microsoft.Extensions.Logging;

namespace KeyCrawler.Service.Services
{
    public class SearchService : ISearchService
    {
        // private readonly ISearchResultsRepository _searchResultsRepo;
        private readonly ILogger<SearchService> _logger;
        private readonly HttpClient _httpClient = new HttpClient(); //TODO make singleton wrapper

        public SearchService(ILogger<SearchService> logger) 
        {
            _logger = logger;
        }

        public async Task Search(IEnumerable<string> uris, IEnumerable<string> keywords)
        {
            foreach(var uri in uris.Select(uri => new Uri(uri)).Distinct()) 
            {
                var pages = await GetAllPages(uri);
                //var keywordsOccurances = GetKeywordsOccurances(keywords, pages);
                // _searchResultsRepo.Add(new SearchResults {
                //     Domain = domain,
                //     KeywordsOccurances = keywordsOccurances
                // });
            }
        }

        private async Task<IEnumerable<HtmlDocument>> GetAllPages(Uri domain) //TODO move this to a separete class
        {
            var pages = new Queue<Uri>();
            var visitedPages = new List<Uri>();

            var depth = 0;
            _logger.LogInformation($"MAIN PAGE {domain}");
            pages.Enqueue(domain);
            while(pages.Any()) //Add subpage limit
            {
                var page = pages.Dequeue();
                visitedPages.Add(page);
                //var html = await GetPageHtml();
                //var subPagesUris = GetSubPagesUris(html);
                var subPages = await GetSubPages(page);

                foreach(var notVisitedPage in subPages.Except(visitedPages).Except(pages)) 
                {
                    _logger.LogWarning(notVisitedPage.AbsoluteUri);
                    pages.Enqueue(notVisitedPage);
                }

                depth++;
                _logger.LogInformation($"Depth {depth}");
            }
            _logger.LogInformation($"DONE FOR PAGE {domain}");
            return null;
        }

        private async Task<IEnumerable<Uri>> GetSubPages(Uri page) {
            ////////////////////
            var response = await _httpClient.GetAsync(page);
            if(!response.IsSuccessStatusCode) {
                return new List<Uri>();
            }
            var content = await response.Content.ReadAsStringAsync();
            var html = new HtmlDocument();
            html.LoadHtml(content); //TODO also return this or move this to a separate method?
            /////////////
            var links = html.DocumentNode.SelectNodes("//a[@href]");
            var subPages = links.Select(link => new Uri(page.Scheme + "://" + page.DnsSafeHost +"/" +link.Attributes["href"].Value)); //TODO make the slash conditional, also don't forget about link with full path and check the domain
            return subPages;
        }

        private IDictionary<string, int> GetKeywordsOccurances(IEnumerable<string> keywords, IEnumerable<string> pages)
        {
            var result = new Dictionary<string, int>();
            foreach(var page in pages) 
            {
                var pageText = ExtractPageText(page).ToLower();
                foreach(var keyword in keywords) 
                {
                    result[keyword] = GetOccurances(keyword, pageText);
                }
            }
            return result;
        }

        private string ExtractPageText(string page)
        {
            throw new NotImplementedException();
        }

        private int GetOccurances(string keyword, string text)
        {
            throw new NotImplementedException();
        }
    }
}
