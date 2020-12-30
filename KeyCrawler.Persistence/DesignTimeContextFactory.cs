using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KeyCrawler.Persistence
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<KeyCrawlerContext> 
    { 
        public KeyCrawlerContext CreateDbContext(string[] args) 
        { 
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($@"{Directory.GetCurrentDirectory()}/../KeyCrawler.WebApi/appsettings.json")
                .Build(); 
            var builder = new DbContextOptionsBuilder<KeyCrawlerContext>(); 
            var connectionString = configuration.GetConnectionString("KeyCrawler"); 
            builder.UseNpgsql<KeyCrawlerContext>(connectionString); 
            return new KeyCrawlerContext(builder.Options); 
        } 
    }
}
