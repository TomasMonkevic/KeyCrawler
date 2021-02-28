using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KeyCrawler.Persistence
{
    public static class ServiceCollectionExtentions
    {
        public static void AddPersistanceLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<KeyCrawlerContext>(options => options.UseNpgsql(connectionString));
        }
    }
}