using KeyCrawler.Domain;
using Microsoft.EntityFrameworkCore;

namespace KeyCrawler.Persistence
{
    public class KeyCrawlerContext : DbContext
    {
        public DbSet<Search> Searches { get; set; }
        public DbSet<UriReport> UriReports { get; set; }

        public KeyCrawlerContext(DbContextOptions<KeyCrawlerContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Search>().HasMany(s => s.UriReports)
                                         .WithOne(r => r.Search)
                                         .HasForeignKey(r => r.SearchId);

            modelBuilder.Entity<Search>().HasIndex(s => s.Id).IsUnique();
        }
    }
}
