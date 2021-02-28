using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeyCrawler.Service.Services
{
    public interface ISearchService
    {
        Task Search(IEnumerable<string> uris, IEnumerable<string> keywords, CancellationToken cancellationToken);
    }
}