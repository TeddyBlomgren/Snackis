namespace Snackis.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public string UserName { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
