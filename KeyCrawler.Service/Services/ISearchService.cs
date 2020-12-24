using System.Collections.Generic;

namespace KeyCrawler.Service.Services
{
    public interface ISearchService
    {
        void Search(IEnumerable<string> uris, IEnumerable<string> keywords);
    }
}
