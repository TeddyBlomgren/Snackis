using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.ViewModels;

namespace Snackis.Pages.Admin
{
    [Authorize(Roles = "MainAdmin,Admin")]
    public class ReportsModel : PageModel
    {
        private readonly ForumDbContext _db;

        public ReportsModel(ForumDbContext db)
        {
            _db = db;
        }

        public IList<ReportViewModel> AllReports { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Hämta alla rapporter
            var reports = await _db.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Post)
                .Include(r => r.Comment)
                .AsNoTracking()
                .ToListAsync();

            AllReports = new List<ReportViewModel>();
            foreach (var r in reports)
            {
                AllReports.Add(new ReportViewModel
                {
                    Id = r.Id,
                    ReporterDisplayName = r.Reporter?.DisplayName ?? "(okänd)",
                    TimeCreated = r.TimeCreated,
                    PostId = r.PostId,
                    PostTitle = r.Post?.Title,
                    CommentId = r.CommentId,
                    CommentText = r.Comment?.Text,
                    IsHandled = r.IsHandled
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostMarkHandledAsync(int reportId)
        {
            var report = await _db.Reports.FindAsync(reportId);
            if (report == null) return NotFound();

            report.IsHandled = true;
            _db.Reports.Update(report);
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int reportId)
        {
            var report = await _db.Reports
                .Include(r => r.Post)
                .Include(r => r.Comment)
                .FirstOrDefaultAsync(r => r.Id == reportId);

            if (report == null) return NotFound();

            if (report.PostId.HasValue && report.Post != null)
            {
                // Radera alla kommentarer på posten
                var comments = await _db.Comments
                    .Where(c => c.PostId == report.Post.Id)
                    .ToListAsync();
                _db.Comments.RemoveRange(comments);

                // Radera posten
                _db.Posts.Remove(report.Post);
            }
            else if (report.CommentId.HasValue && report.Comment != null)
            {
                _db.Comments.Remove(report.Comment);
            }

            _db.Reports.Remove(report);
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
