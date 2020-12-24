using System.Collections.Generic;

namespace KeyCrawler.WebApi.V1.Requests
{
    public class SearchRequest
    {
        public IEnumerable<string> Uris { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}
