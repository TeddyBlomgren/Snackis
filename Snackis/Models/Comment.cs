using System;
using System.ComponentModel.DataAnnotations;

namespace Snackis.Models

{
    public class Comment
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Kommentaren får inte vara tom")]
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
