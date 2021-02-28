using System.Collections.Generic;

namespace KeyCrawler.Domain
{
    public class Search
    {
        public string Id { get; set; }
        public string JobId { get; set; }
        public IEnumerable<UriReport> UriReports { get; set; }
    }
}