﻿namespace Snackis.ViewModels
{
    public class UserRoles
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
