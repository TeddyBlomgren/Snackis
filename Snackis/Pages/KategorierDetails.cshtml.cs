using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data;
using Snackis.Models;
using System.Linq;

namespace Snackis.Pages
{
    public class KategorierDetailsModel : PageModel
    {
        private readonly ForumDbContext _context;

        public KategorierDetailsModel(ForumDbContext context)
        {
            _context = context;
        }

        public string CategoryName { get; set; }
        public List<Post> Posts { get; set; }

        public void OnGet(string name)
        {
            CategoryName = name;

           
            Posts = _context.Posts
                .Where(p => p.Category.Name == name)
                .ToList();
        }
    }
}
