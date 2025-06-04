using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly ForumDbContext _context;

        public IndexModel(UserManager<SnackisUser> userManager, ForumDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string? DisplayName { get; set; }
        public Dictionary<string, List<Post>> LatestPostsByCategory { get; set; }

        public List<Post> LatestPosts { get; set; }

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                DisplayName = user?.DisplayName;
            }

            LatestPosts = await _context.Posts
                .OrderByDescending(p => p.Date)
                .Take(5)
                .Include(p => p.User)
                .ToListAsync();
        }


    }
}
