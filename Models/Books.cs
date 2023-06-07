using Newtonsoft.Json;
namespace ReadingDiaryApi.Models
{
    public class Books
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("items")]
        public List<Book> Items { get; set; }
    }
}
