using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.DAL;
using Snackis.Data;
using Snackis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Pages
{
    [Authorize(Roles = "MainAdmin,Admin,User")]
    public class MessageModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;

        public MessageModel(UserManager<SnackisUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; } 

        [BindProperty]
        public string RecipientId { get; set; }

        [BindProperty]
        public string MessageText { get; set; } 

        public string CurrentUserId { get; set; }
        public SnackisUser Contact { get; set; }
        public List<PM> Messages { get; set; } = new List<PM>();

        public List<SnackisUser> ChatContacts { get; set; } = new List<SnackisUser>();  
        public List<SnackisUser> AllUsers { get; set; } = new List<SnackisUser>(); 

        public async Task OnGetAsync()
        {

            CurrentUserId = _userManager.GetUserId(User);


            var inbox = await PmApiManager.GetInboxAsync(CurrentUserId);
            var outbox = await PmApiManager.GetOutBoxAsync(CurrentUserId);

            var allMessages = inbox.Concat(outbox).ToList();
            var contactIds = allMessages
                .Select(m => m.SenderId == CurrentUserId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .Where(id => id != CurrentUserId)
                .ToList();

            if (contactIds.Any())
            {
                ChatContacts = _userManager.Users
                    .Where(u => contactIds.Contains(u.Id))
                    .ToList();
            }

            AllUsers = _userManager.Users
                .Where(u => u.Id != CurrentUserId)
                .ToList();

           
            if (!string.IsNullOrEmpty(UserId))
            {
                Contact = await _userManager.FindByIdAsync(UserId);
                Messages = await PmApiManager.GetConversationAsync(CurrentUserId, UserId);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUserId = _userManager.GetUserId(User);

            var pm = new PM
            {
                SenderId = CurrentUserId,
                ReceiverId = RecipientId,
                Content = MessageText,
                SentAt = System.DateTime.Now
            };

            await PmApiManager.SendMessageAsync(pm);

            return RedirectToPage("/Message", new { userId = RecipientId });
        }
    }
}
