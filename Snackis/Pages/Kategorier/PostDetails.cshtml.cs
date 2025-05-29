using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Pages.Kategorier
{
    public class PostDetailsModel : PageModel
    {
        private readonly ForumDbContext _context;
        private readonly UserManager<SnackisUser> _userManager;

        public PostDetailsModel(ForumDbContext context, UserManager<SnackisUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Post Post { get; set; }

        [BindProperty]
        public CommentInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Console.WriteLine("DEBUG: OnPostAsync startar, id = " + id);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("DEBUG: Ingen användare hittades, Unauthorized.");
                return Unauthorized();
            }

            var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                Console.WriteLine("DEBUG: Inget inlägg hittades med id = " + id);
                return NotFound();
            }

            Console.WriteLine("DEBUG: ModelState.IsValid = " + ModelState.IsValid);
            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"DEBUG: ModelState-fel i {kvp.Key}: {error.ErrorMessage}");
                    }
                }

                Post = post;
                return Page();
            }

            var newComment = new Comment
            {
                PostId = id,
                UserId = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Date = DateTime.Now,
                Text = Input.Text
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            Console.WriteLine("DEBUG: Kommentar sparad och redirect körs.");
            return RedirectToPage("./PostDetails", new { id = id });
        }
    }
}
