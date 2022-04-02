using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokayListMaker.models
{
    public class Snippet
    {
        [JsonProperty("publishedAt")]
        public DateTimeOffset? PublishedAt { get; set; }
        [JsonProperty("channelId")]
        public string? ChannelId { get; set; }
        [JsonProperty("title")]
        public string? Title { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("thumbnails")]
        public Thumbnail? Thumbnails { get; set; }
    }
    public class Thumbnail
    {
        [JsonProperty("default")]
        public ThumbnailDetail? Default { get; set; }
        [JsonProperty("medium")]
        public ThumbnailDetail? Medium { get; set; }
        [JsonProperty("high")]
        public ThumbnailDetail? High { get; set; }
        [JsonProperty("standard")]
        public ThumbnailDetail? Standard { get; set; }
        [JsonProperty("maxres")]
        public ThumbnailDetail? MaxRes { get; set; }
    }

    public class ThumbnailDetail
    {
        [JsonProperty("url")]
        public string? Url { get; set; }
        [JsonProperty("width")]
        public int? Width { get; set; }
        [JsonProperty("height")]
        public int? Height { get; set; }
    }
}
