using System.Net;
using Google.Apis.YouTube.v3;
using ReadingDiaryApi.Constants;
using ReadingDiaryApi.Models;
using Newtonsoft.Json;

namespace ReadingDiaryApi.Clients
{
    public class Client
    {
        private HttpClient _client;
        private static string _addressBook;
        private static string _addressAudio;
        private static string _key;

        public Client()
        {
            _addressBook = Constant.addressBook;
            _addressAudio = Constant.addressAudio;
            _key = Constant.apikey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_addressBook);
        }
        public async Task<Book> GetBookAsync(string name, int bookIndex)
        {
            var respons = await _client.GetAsync($"volumes?q={name}&key={_key}");
            respons.EnsureSuccessStatusCode();
            var content = respons.Content.ReadAsStringAsync().Result;
            var books = JsonConvert.DeserializeObject<Books>(content);

            if (bookIndex >= 0 && bookIndex < books.Items.Count)
            {
                return books.Items[bookIndex];
            }

            return null; 
        }
        public async Task<Book> GetAuthorBookAsync(string name, string author, int bookIndex)
        {
            var respons = await _client.GetAsync($"volumes?q={name}+inauthor:{author}&key={_key}");
            respons.EnsureSuccessStatusCode();
            var content = respons.Content.ReadAsStringAsync().Result;
            var books = JsonConvert.DeserializeObject<Books>(content);

            if (bookIndex >= 0 && bookIndex < books.Items.Count)
            {
                return books.Items[bookIndex];
            }

            return null;
        }

        public async Task<string> GetYouTubeVideo(string videoTitle)
        { 

            var encodedTitle = Uri.EscapeDataString(videoTitle);
            var requestUrl = $"{_addressAudio}/search?key={_key}&q={encodedTitle}&type=video&maxResults=1";

            var response = await _client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<YouTube>(content);

            if (result?.Items?.Count > 0)
            {
                var videoId = result.Items[0].Id.VideoId;
                var videoLink = $"https://www.youtube.com/watch?v={videoId}";
                return videoLink;
            }

            return null;
        }
    }
}
