using KeyCrawler.Domain;

namespace KeyCrawler.Persistence.Repositories
{
    public interface ISearchResultsRepository
    {
        void Add(SearchResults searchResults);
    }
}
