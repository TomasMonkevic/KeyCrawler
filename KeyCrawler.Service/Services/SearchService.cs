using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KeyCrawler.Domain;
using KeyCrawler.Persistence.Repositories;
using KeyCrawler.Service.Utils;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace KeyCrawler.Service.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUriReportRepository _uriReportRepository;
        private readonly IPageFetcher _pageFetcher;
        private readonly ILogger<SearchService> _logger;

        public SearchService(IUriReportRepository searchResultsRepo, IPageFetcher pageFetcher, ILogger<SearchService> logger) 
        {
            _uriReportRepository = searchResultsRepo;
            _pageFetcher = pageFetcher;
            _logger = logger;
        }

        public async Task Search(IEnumerable<string> uris, IEnumerable<string> keywords, CancellationToken cancellationToken)
        {
            foreach(var uri in uris.Select(uri => new Uri(uri)).Distinct()) 
            {
                //TODO later check if this uri was handled for these keywords
                var pages = await _pageFetcher.GetAllPages(uri, cancellationToken);
                var keywordsOccurances = GetKeywordsOccurances(keywords, pages);
                _uriReportRepository.Add(new UriReport {
                    Uri = uri.AbsoluteUri,
                    KeywordsOccurances = keywordsOccurances
                });
            }
        }

        private IDictionary<string, int> GetKeywordsOccurances(IEnumerable<string> keywords, IEnumerable<HtmlDocument> pages)
        {
            var result = new Dictionary<string, int>();
            foreach(var page in pages) 
            {
                var pageText = ExtractPageText(page);
                foreach(var keyword in keywords) 
                {
                    if(result.ContainsKey(keyword))
                    {
                        result[keyword] += GetOccurances(pageText, keyword);
                    }
                    else 
                    {
                        result[keyword] = GetOccurances(pageText, keyword);
                    }
                }
            }
            return result;
        }

        private string ExtractPageText(HtmlDocument page)
        {
            var pattern = new Regex("[\t\r]");
            var pageText = page.DocumentNode.SelectSingleNode("//body").InnerText;
            return pattern.Replace(pageText, "");
        }

        private int GetOccurances(string text, string keyword)
        {
            var escapedKeyword = Regex.Escape(keyword); //user regex not keywords
            return Regex.Matches(text, escapedKeyword, RegexOptions.IgnoreCase).Count;
        }
    }
}
