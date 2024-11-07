using Microsoft.EntityFrameworkCore;
using News.Models;

namespace News.Data
{
    public class NewsDbContext : DbContext
    {

        public NewsDbContext(DbContextOptions<NewsDbContext> Options):base(Options){}
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>()
                .Property(x => x.Title)
                .HasMaxLength(70)
                .HasColumnType("text");

            modelBuilder.Entity<Category>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired(true);
        }

    }
}
