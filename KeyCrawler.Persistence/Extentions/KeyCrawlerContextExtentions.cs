using Microsoft.EntityFrameworkCore;

namespace KeyCrawler.Persistence.Extentions
{
    public static class KeyCrawlerContextExtentions
    {
        public static void Migrate(this KeyCrawlerContext context)
        {
            context.Database.Migrate();
        }
    }
}