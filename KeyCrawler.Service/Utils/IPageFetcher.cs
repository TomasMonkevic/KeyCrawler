using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace KeyCrawler.Service.Utils
{
    public interface IPageFetcher
    {
        Task<HtmlDocument> GetPage(Uri uri, CancellationToken cancellationToken);
        Task<IEnumerable<HtmlDocument>> GetAllPages(Uri domain, CancellationToken cancellationToken);
        IEnumerable<Uri> GetUris(Uri uri, HtmlDocument page);
    }
}
