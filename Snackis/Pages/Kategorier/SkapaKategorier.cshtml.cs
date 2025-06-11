using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data;
using Snackis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Snackis.Pages.Kategorier
{
    [Authorize(Roles = "MainAdmin, Admin")]
    public class SkapaKategorierModel : PageModel
    {
        private readonly ForumDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SkapaKategorierModel(ForumDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Category Category { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {


            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var saveFolder = Path.Combine(_env.WebRootPath, "images", "kategorier");
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                var savePath = Path.Combine(saveFolder, fileName);
                using (var filStream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(filStream);
                }

                Category.Image = "/images/kategorier/" + fileName;
            }
            else
            {

                Category.Image = "";

            }

            if (!ModelState.IsValid)
            {

                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"ModelState-fel för '{entry.Key}': {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var exists = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == Category.Name.ToLower());
            if (exists != null)
            {
                ModelState.AddModelError(string.Empty, "Kategorin finns redan.");
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("Kategorier");
        }


    }
}
