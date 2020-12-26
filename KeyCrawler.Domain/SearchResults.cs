using System.Collections.Generic;

namespace KeyCrawler.Domain
{
    public class SearchResults
    {
        public string Uri { get; set; }
        public IDictionary<string, int> KeywordsOccurances { get; set; }
    }
}
