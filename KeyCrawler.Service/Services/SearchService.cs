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

namespace KeyCrawler.Service.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchResultsRepository _searchResultsRepo;
        private readonly IPageFetcher _pageFetcher;
        private readonly ILogger<SearchService> _logger;

        public SearchService(ISearchResultsRepository searchResultsRepo, IPageFetcher pageFetcher, ILogger<SearchService> logger) 
        {
            _searchResultsRepo = searchResultsRepo;
            _pageFetcher = pageFetcher;
            _logger = logger;
        }

        public async Task Search(IEnumerable<string> uris, IEnumerable<string> keywords)
        {
            foreach(var uri in uris.Select(uri => new Uri(uri)).Distinct()) 
            {
                var pages = await _pageFetcher.GetAllPages(uri);
                var keywordsOccurances = GetKeywordsOccurances(keywords, pages);
                _searchResultsRepo.Add(new SearchResults {
                    Uri = uri.AbsoluteUri,
                    KeywordsOccurances = keywordsOccurances
                });
            }
        }

        private IDictionary<string, int> GetKeywordsOccurances(IEnumerable<string> keywords, IEnumerable<HtmlDocument> pages)
        {
            var result = new Dictionary<string, int>();
            _logger.LogError(pages.Count().ToString());
            foreach(var page in pages) 
            {
                var pageText = ExtractPageText(page);
                _logger.LogError(pageText);
                foreach(var keyword in keywords) 
                {
                    result[keyword] = GetOccurances(pageText, keyword);
                }
            }
            return result;
        }

        private string ExtractPageText(HtmlDocument page)
        {
            return page.DocumentNode.SelectSingleNode("//body").InnerText;
        }

        private int GetOccurances(string text, string keyword)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            text = Encoding.UTF8.GetString(bytes);
            var escapedKeyword = Regex.Escape(keyword.ToLower());
            return Regex.Matches(text.ToLower(), escapedKeyword).Count;
        }
    }
}
