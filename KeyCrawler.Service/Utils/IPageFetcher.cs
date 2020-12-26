using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace KeyCrawler.Service.Utils
{
    public interface IPageFetcher
    {
        Task<HtmlDocument> GetPage(Uri uri);
        Task<IEnumerable<HtmlDocument>> GetAllPages(Uri domain);
        IEnumerable<Uri> GetUris(Uri uri, HtmlDocument page);
    }
}
