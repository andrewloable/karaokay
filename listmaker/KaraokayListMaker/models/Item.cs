using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokayListMaker.models
{
    public class Item
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }
        [JsonProperty("etag")]
        public string? Etag { get; set; }
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("snippet")]
        public Snippet? Snippet { get; set; }
    }
}
