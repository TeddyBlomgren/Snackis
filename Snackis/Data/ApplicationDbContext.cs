using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Data
{
    public class ApplicationDbContext : IdentityDbContext<SnackisUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SnackisUser> SnackisUsers { get; set; } 
    }
}
