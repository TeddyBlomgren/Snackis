using System.Text.Json.Serialization;

namespace Snackis.Models
{
    public class PM
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("senderId")]
        public string SenderId { get; set; }
        [JsonPropertyName("receiverId")]
        public string ReceiverId { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("sentAt")]
        public DateTime SentAt { get; set; } = DateTime.Now;
        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; } = false;
    }
}
