using System;
using KeyCrawler.Domain;
using Microsoft.Extensions.Logging;

namespace KeyCrawler.Persistence.Repositories
{
    public class SearchResultsRepository : ISearchResultsRepository
    {
        private readonly ILogger<SearchResultsRepository> _logger;

        public SearchResultsRepository(ILogger<SearchResultsRepository> logger)
        {
            _logger = logger;
        }

        public void Add(SearchResults searchResults)
        {
            _logger.LogInformation(searchResults.Uri);
            foreach(var pair in searchResults.KeywordsOccurances) 
            {
                _logger.LogInformation($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
