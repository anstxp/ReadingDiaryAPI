using Newtonsoft.Json;
namespace ReadingDiaryApi.Models
{
    public class YouTube
    {
        public List<YouTubeVideoItem> Items { get; set; }
        public YouTubeVideoId Id { get; set; }
    }

    public class YouTubeVideoItem
    {
        public YouTubeVideoId Id { get; set; }
    }

    public class YouTubeVideoId
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }

}
