using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokayListMaker.models
{
    public class KListItem
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("title")]
        public string? Title { get; set; }
        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }
        [JsonProperty("duration")]
        public int? Duration { get; set; }
  }
}
