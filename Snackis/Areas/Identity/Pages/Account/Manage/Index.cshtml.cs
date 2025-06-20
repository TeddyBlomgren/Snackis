﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data;

namespace Snackis.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;

        public IndexModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public IFormFile UploadImage { get; set; }

        public string CurrentImage { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(SnackisUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            CurrentImage = string.IsNullOrEmpty(user.ProfileImage)
                ? "/images/Anon.png"
                : user.ProfileImage;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Hantera telefonnummer
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Hantera bilduppladdning
            if (UploadImage != null && UploadImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp" };
                var fileExtension = Path.GetExtension(UploadImage.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("UploadImage", "Endast bildfiler (jpg, jpeg, png, bmp, webp) är tillåtna.");
                    return Page();
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadImage.CopyToAsync(stream);
                }

                user.ProfileImage = $"/uploads/{fileName}";
            }



            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
