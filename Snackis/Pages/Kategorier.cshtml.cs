using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis.Pages
{
    public class KategorierModel : PageModel
    {
        public List<Models.Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = new List<Models.Category>
            {
                new Models.Category { Name = "Bilar", Description = "Diskutera bilar, motorer och allt som rullar på fyra hjul.", Image = "/images/Enfinbil.jpeg" },
                new Models.Category { Name = "Båtar", Description = "För dig som älskar båtar, segling och havets äventyr.", Image = "/images/Enfinbat.jpeg" },
                new Models.Category { Name = "Spel", Description = "Allt om spel – konsol, PC och mobil, tips och diskussioner.", Image = "/images/Enspelbild.jpeg" },
                new Models.Category { Name = "Sport", Description = "Prata om sport, matcher, träning och dina favoritlag.", Image = "/images/Ensportigbild.jpeg" }
            };
        }
    }


}
