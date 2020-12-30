using System;
using KeyCrawler.Domain;
using Microsoft.Extensions.Logging;

namespace KeyCrawler.Persistence.Repositories
{
    public class UriReportRepository : IUriReportRepository
    {
        private readonly ILogger<UriReportRepository> _logger;

        public UriReportRepository(ILogger<UriReportRepository> logger)
        {
            _logger = logger;
        }

        public void Add(UriReport searchResults)
        {
            _logger.LogInformation(searchResults.Uri);
            foreach(var pair in searchResults.KeywordsOccurances) 
            {
                _logger.LogInformation($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
