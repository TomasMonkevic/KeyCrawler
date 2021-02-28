using KeyCrawler.Domain;

namespace KeyCrawler.Persistence.Repositories
{
    public interface IUriReportRepository
    {
        void Add(UriReport searchResults);
    }
}