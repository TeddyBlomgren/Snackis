using Microsoft.EntityFrameworkCore;
using Snackis.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Snackis.Data
{
    public class ForumDbContext : IdentityDbContext<SnackisUser>
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options)
            : base(options)
        {
        }
        public DbSet<Snackis.Models.Post> Posts { get; set; }
        public DbSet<Snackis.Models.Comment> Comments { get; set; }
        public DbSet<Snackis.Models.Category> Categories { get; set; }
        public DbSet<Snackis.Models.Report> Reports { get; set; }
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

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
           new Category
           {
               Id = 1,
               Name = "Spel",
               Description = "Allt inom spel – konsol, PC och mobil",
               Image = "/images/EnSpelbild.jpeg"
           },
           new Category
           {
               Id = 2,
               Name = "Sport",
               Description = "Allt om sport och träning",
               Image = "/images/EnSportigbild.jpeg"
           },
           new Category
           {
               Id = 3,
               Name = "Bilar",
               Description = "Allt om bilar och byggen",
               Image = "/images/EnFinBil.jpeg"
           },
           new Category
           {
               Id = 4,
               Name = "Båtar",
               Description = "Allt om båtar och sjöliv",
               Image = "/images/EnFinBat.jpeg"
           }
           );
        }

    }
}
