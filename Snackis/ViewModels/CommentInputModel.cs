using System.ComponentModel.DataAnnotations;

namespace Snackis.ViewModels
{
    public class CommentInputModel
    {
        [Required(ErrorMessage = "Kommentaren får inte vara tom")]
        public string Text { get; set; }
    }
}

