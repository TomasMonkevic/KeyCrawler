using System.Collections.Generic;

namespace KeyCrawler.Domain
{
    public class SearchResults
    {
        public string Domain { get; set; }
        public IDictionary<string, int> KeywordsOccurances { get; set; }
    }
}
