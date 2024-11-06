using Microsoft.EntityFrameworkCore;
using News.Models;

namespace News.Data
{
    public class NewsDbContext : DbContext
    {
        private readonly DbContextOptions options;

        public NewsDbContext(DbContextOptions<NewsDbContext> Options):base()
        {
            options = Options;
        }
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
