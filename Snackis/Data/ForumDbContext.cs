using Microsoft.EntityFrameworkCore;

namespace Snackis.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options)
            : base(options)
        {
        }
        public DbSet<Snackis.Models.Post> Posts { get; set; }
        public DbSet<Snackis.Models.Comment> Comments { get; set; }
        public DbSet<Snackis.Models.Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Snackis.Models.Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Snackis.Models.Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);
        }

    }
}
