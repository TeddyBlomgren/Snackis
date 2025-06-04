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
        public string UserId { get; set; }      // Den du pratar med (fr�n query)

        [BindProperty]
        public string RecipientId { get; set; } // F�r nytt PM (fr�n formul�r)

        [BindProperty]
        public string MessageText { get; set; } // �ndrat namn fr�n "Content" till "MessageText"

        public string CurrentUserId { get; set; }
        public SnackisUser Contact { get; set; }
        public List<PM> Messages { get; set; } = new List<PM>();

        public List<SnackisUser> ChatContacts { get; set; } = new List<SnackisUser>();  // De anv�ndare du redan har chattat med
        public List<SnackisUser> AllUsers { get; set; } = new List<SnackisUser>();      // Alla anv�ndare f�r dropdown

        public async Task OnGetAsync()
        {
            // 1) H�mta inloggad anv�ndares ID
            CurrentUserId = _userManager.GetUserId(User);

            // 2) H�mta b�de inbox (mottagna) och outbox (skickade) � enbart f�r att bygga listan ChatContacts
            var inbox = await PmApiManager.GetInboxAsync(CurrentUserId);
            var outbox = await PmApiManager.GetOutBoxAsync(CurrentUserId);

            // 3) Bygg en lista med unika kontakt-IDn (alla som du skickat till eller f�tt fr�n)
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

            // 4) H�mta alla anv�ndare (utom du sj�lv) f�r dropdown n�r man vill starta en ny PM
            AllUsers = _userManager.Users
                .Where(u => u.Id != CurrentUserId)
                .ToList();

            // 5) Om en specifik chatt �r vald (UserId != null), h�mta hela konversationen via nya endpointen
            if (!string.IsNullOrEmpty(UserId))
            {
                Contact = await _userManager.FindByIdAsync(UserId);

                // Anropa API-metoden som ger hela chattr�den i ett steg:
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

            // Redirecta tillbaka till samma sida, med UserId satt till den vi nyss skickade till
            return RedirectToPage("/Message", new { userId = RecipientId });
        }
    }
}
