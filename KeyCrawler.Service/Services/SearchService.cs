using System;
using System.Collections.Generic;
using System.Linq;
using KeyCrawler.Domain;

namespace KeyCrawler.Service.Services
{
    public class SearchService : ISearchService
    {
        // private readonly ISearchResultsRepository _searchResultsRepo;

        // public SearchService(ISearchResultsRepository searchResultsRepo) 
        // {
        //     searchResultsRepo = _searchResultsRepo;
        // }

        public void Search(IEnumerable<string> uris, IEnumerable<string> keywords)
        {
            var domains = uris.Select(uri => new Uri(uri).DnsSafeHost).Distinct();
            foreach(var domain in domains) 
            {
                var pages = GetAllPages(domain);
                var keywordsOccurances = GetKeywordsOccurances(keywords, pages);
                // _searchResultsRepo.Add(new SearchResults {
                //     Domain = domain,
                //     KeywordsOccurances = keywordsOccurances
                // });
            }
        }

        private IEnumerable<string> GetAllPages(string domain) //TODO move this to a separete class
        {
            throw new NotImplementedException();
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
