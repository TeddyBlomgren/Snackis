using Microsoft.AspNetCore.Identity;

namespace Snackis.Data
{
    public class SnackisUser : IdentityUser
    {
        [PersonalData]
        public string? DisplayName { get; set; }
        [PersonalData]
        public int BirthYear { get; set; }
        [PersonalData]
        public string? Name { get; set; }
        
        [PersonalData]
        public string? LastName { get; set; }
        
        [PersonalData]
        public string ProfileImage { get; set; } = "/images/Anon.png";
    }
}
