using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var postFromDb = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Post = postFromDb;
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

        // Rapportera inlägg
        public async Task<IActionResult> OnPostReportPostAsync()
        {

            if (Post == null || Post.Id == 0)
            {
                return BadRequest();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var report = new Report
            {
                ReporterId = currentUser.Id,
                PostId = Post.Id,
                TimeCreated = DateTime.UtcNow,
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PostDetails", new { id = Post.Id });
        }

        // rapportera kommentar
        public async Task<IActionResult> OnPostReportCommentAsync(int commentId)
        {
            if (commentId == 0)
            {
                return BadRequest();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var report = new Report
            {
                ReporterId = currentUser.Id,
                CommentId = commentId,
                TimeCreated = DateTime.UtcNow,
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PostDetails", new { id = Post.Id });
        }
    }
}
