using Snackis.Models;
using System.Text.Json;

namespace Snackis.DAL
{
    public class PmApiManager
    {
        private static readonly Uri BaseAdress = new Uri("https://localhost:44329/");

        public static async Task<List<PM>> GetInboxAsync(string userId)
        {
            List<PM> inbox = new List<PM>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAdress;
                HttpResponseMessage response = await client.GetAsync($"api/PM/inbox/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    inbox = JsonSerializer.Deserialize<List<PM>>(responseString);
                }
                return inbox;
            }
        }
        public static async Task<List<PM>> GetOutBoxAsync(string userId)
        {
            List<PM> outbox = new List<PM>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAdress;
                HttpResponseMessage response = await client.GetAsync($"api/PM/outbox/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    outbox = JsonSerializer.Deserialize<List<PM>>(responseString);
                }
                return outbox;
            }
        }
        public static async Task<PM> GetMessageAsync(int id)
        {
            PM message = new();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAdress;
                HttpResponseMessage response = await client.GetAsync($"api/PM/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    message = JsonSerializer.Deserialize<PM>(responseString);
                }
                return message;
            }
        }
        public static async Task SendMessageAsync(PM message)
        {
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAdress;
                var json = JsonSerializer.Serialize(message);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync("api/PM", httpContent);

            }
        }
        public static async Task DeleteMessageAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAdress;
                await client.DeleteAsync($"api/PM/{id}");
            }
        }
        public static async Task MarkAsReadAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAdress;
                await client.PutAsync($"api/PM/markasread/{id}", null);
            }
        }
    }
}
