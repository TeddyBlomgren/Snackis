// Pages/Kategorier/PostDetails.cshtml.cs
using System;
using System.Linq;
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
    [Authorize]
    public class PostDetailsModel : PageModel
    {
        private readonly ForumDbContext _db;
        private readonly UserManager<SnackisUser> _users;

        private static readonly TimeZoneInfo SwedishZone =
            TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

        public PostDetailsModel(ForumDbContext db, UserManager<SnackisUser> users)
        {
            _db = db;
            _users = users;
        }

        public Post Post { get; private set; }

        [BindProperty]
        public CommentInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments.OrderByDescending(c => c.Date))
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Post == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(id);
                return Page();
            }

            var user = await _users.GetUserAsync(User)
                       ?? throw new InvalidOperationException("Ej inloggad");


            var commentTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                SwedishZone
            );

            _db.Comments.Add(new Comment
            {
                PostId = id,
                UserId = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName ?? user.UserName,
                Date = commentTime,
                Text = Input.Text
            });

            await _db.SaveChangesAsync();
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostReportPostAsync(int id)
        {
            var user = await _users.GetUserAsync(User)
                       ?? throw new InvalidOperationException("Ej inloggad");

            var reportTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                SwedishZone
            );

            _db.Reports.Add(new Report
            {
                ReporterId = user.Id,
                PostId = id,
                TimeCreated = reportTime
            });
            await _db.SaveChangesAsync();
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostReportCommentAsync(int id, int commentId)
        {
            var user = await _users.GetUserAsync(User)
                       ?? throw new InvalidOperationException("Ej inloggad");

            var reportTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                SwedishZone
            );

            _db.Reports.Add(new Report
            {
                ReporterId = user.Id,
                CommentId = commentId,
                TimeCreated = reportTime
            });
            await _db.SaveChangesAsync();
            return RedirectToPage(new { id });
        }
    }
}
