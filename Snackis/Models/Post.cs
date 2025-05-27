using Microsoft.AspNetCore.Mvc.ModelBinding;
using Snackis.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snackis.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        public string? Image { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [BindNever]
        public string? UserId { get; set; }

        [BindNever]
        public string? UserName { get; set; }

        public int CategoryId { get; set; }
        public SnackisUser? User { get; set; }


        [BindNever]
        public Category? Category { get; set; }

        [BindNever]
        public List<Comment>? Comments { get; set; }
    }
}
