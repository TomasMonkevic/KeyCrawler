using KeyCrawler.Domain;

namespace KeyCrawler.Persistence.Repositories
{
    public class UriReportRepository : IUriReportRepository
    {
        private readonly KeyCrawlerContext _db;

        public UriReportRepository(KeyCrawlerContext databaseContext)
        {
            _db = databaseContext;
        }

        public void Add(UriReport searchResults)
        {
            _db.UriReports.Add(searchResults);
            _db.SaveChanges();
        }
    }
}