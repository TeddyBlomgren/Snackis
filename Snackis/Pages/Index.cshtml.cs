using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data;

namespace Snackis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        public IndexModel(UserManager<SnackisUser> userManager)
        {
            _userManager = userManager;
        }
        public string? DisplayName { get; set; }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                DisplayName = user?.DisplayName;

            }

        }
    }
}
