using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;
using Snackis.ViewModels;

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

        [BindProperty]
        public Post Post { get; set; }

        [BindProperty]
        public CommentInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context.Posts
               .Include(p => p.User)
               .Include(p => p.Comments)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (Post == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Forbid();

            if (!ModelState.IsValid)
            {
                Post = await _context.Posts
                   .Include(p => p.User)
                   .Include(p => p.Comments.OrderByDescending(c => c.Date))
                   .FirstOrDefaultAsync(p => p.Id == id);
                return Page();
            }

            var newComment = new Comment
            {
                PostId = id,
                UserId = currentUser.Id,
                UserName = currentUser.UserName,
                DisplayName = currentUser.DisplayName,
                Date = DateTime.Now,
                Text = Input.Text
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PostDetails", new { id });
        }

        public async Task<IActionResult> OnPostReportPostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Forbid();

            _context.Reports.Add(new Report
            {
                ReporterId = currentUser.Id,
                PostId = post.Id,
                TimeCreated = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./PostDetails", new { id = post.Id });
        }

        public async Task<IActionResult> OnPostReportCommentAsync(int id, int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Forbid();

            _context.Reports.Add(new Report
            {
                ReporterId = currentUser.Id,
                CommentId = comment.Id,
                TimeCreated = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./PostDetails", new { id });
        }
    }
}
