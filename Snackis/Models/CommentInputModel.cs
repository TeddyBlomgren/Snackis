using System.ComponentModel.DataAnnotations;

namespace Snackis.Models
{
    public class CommentInputModel
    {
        [Required(ErrorMessage = "Kommentaren får inte vara tom")]
        public string Text { get; set; }
    }
}

