using System.Collections.Generic;

namespace KeyCrawler.Domain
{
    public class UriReport
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public IEnumerable<Match> Matches { get; set; }

        public Search Search { get; set; }
        public string SearchId { get; set; }
    }
}