using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.ViewModels;


public class UserRolesModel : PageModel
{
    private readonly UserManager<SnackisUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRolesModel(
        UserManager<SnackisUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<UserRoles> Users { get; set; }
    public List<string> AllRoles { get; set; }

    [BindProperty]
    public string SelectedUserId { get; set; }

    [BindProperty]
    public string SelectedRole { get; set; }

    public async Task OnGetAsync()
    {
        AllRoles = await _roleManager.Roles
                            .Select(r => r.Name)
                            .ToListAsync();

        var userList = await _userManager.Users
                              .ToListAsync();

        Users = new List<UserRoles>();
        foreach (var u in userList) 
        {
            var roles = await _userManager.GetRolesAsync(u);
            Users.Add(new UserRoles
            {
                UserId = u.Id,
                Email = u.Email,
                Roles = roles
            });
        }
    }



    public async Task<IActionResult> OnPostAsync()
    {
        if (SelectedUserId == null || SelectedRole == null)
            return RedirectToPage(); 

        var user = await _userManager.FindByIdAsync(SelectedUserId);
        if (user == null) return NotFound();


        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);


        await _userManager.AddToRoleAsync(user, SelectedRole);

        return RedirectToPage();
    }
}
