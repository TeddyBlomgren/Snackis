using Snackis.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snackis.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReporterId { get; set; }

        [ForeignKey(nameof(ReporterId))]
        public SnackisUser Reporter { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now;

        public int? PostId { get; set; }


        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        public int? CommentId { get; set; }


        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
        public bool IsHandled { get; set; } = false;
    }
}
