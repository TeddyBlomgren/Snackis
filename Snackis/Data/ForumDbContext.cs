using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Snackis.Models;

namespace Snackis.Data
{
    public class ForumDbContext : IdentityDbContext<SnackisUser>
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Spel", Description = "Allt inom spel – konsol, PC och mobil", Image = "/images/EnSpelbild.jpeg" },
                new Category { Id = 2, Name = "Sport", Description = "Allt om sport och träning", Image = "/images/EnSportigbild.jpeg" },
                new Category { Id = 3, Name = "Bilar", Description = "Allt om bilar och byggen", Image = "/images/EnFinBil.jpeg" },
                new Category { Id = 4, Name = "Båtar", Description = "Allt om båtar och sjöliv", Image = "/images/EnFinBat.jpeg" }
            );

        }
    }
}
