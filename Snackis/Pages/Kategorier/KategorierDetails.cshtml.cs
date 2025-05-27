using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Pages.Kategorier
{
    public class KategorierDetailsModel : PageModel
    {
        private readonly ForumDbContext _context;
        private readonly UserManager<SnackisUser> _userManager;

        public KategorierDetailsModel(ForumDbContext context, UserManager<SnackisUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string name { get; set; }

        public List<Post> Posts { get; set; }

        [BindProperty]
        public Post NewPost { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet()
        {
            Posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.Category.Name == name)
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            var cat = _context.Categories.FirstOrDefault(c => c.Name == name);
            if (cat == null)
            {
                return NotFound();
            }

            NewPost.CategoryId = cat.Id;
            NewPost.Date = DateTime.Now;
            NewPost.UserId = user.Id;
            NewPost.UserName = user.UserName;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(fs);

                NewPost.Image = "/uploads/" + fileName;
            }

            _context.Posts.Add(NewPost);
            await _context.SaveChangesAsync();

            return RedirectToPage("./KategorierDetails", new { name = name });
        }
    }
}
