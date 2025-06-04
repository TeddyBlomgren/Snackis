using System;

namespace Snackis.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string ReporterDisplayName { get; set; }
        public DateTime TimeCreated { get; set; }

        public int? PostId { get; set; }
        public string PostTitle { get; set; }

        public int? CommentId { get; set; }
        public string CommentText { get; set; }

        public bool IsHandled { get; set; }
    }
}
