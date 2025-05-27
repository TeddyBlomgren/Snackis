using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data;
using Snackis.Models;
using System.Collections.Generic;

namespace Snackis.Pages.Kategorier
{
    public class KategorierModel : PageModel
    {
        private readonly ForumDbContext _context;
        public List<Category> Categories { get; set; }
        public KategorierModel(ForumDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            // Hämta alla kategorier från databasen
            Categories = _context.Categories
                 .OrderBy(c => c.Name)
                 .ToList();
        }

    }
}
