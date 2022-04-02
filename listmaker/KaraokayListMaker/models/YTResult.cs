using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokayListMaker.models
{
    public class YTResult
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }
        [JsonProperty("etag")]
        public string? Etag { get; set; }
        [JsonProperty("items")]
        public Item[]? Items { get; set; }
    }

    public class PageInfo
    {
        [JsonProperty("totalResults")]
        public int? TotalResults { get; set; }
        [JsonProperty("resultsPerPage")]
        public int? ResultsPerPage { get; set; }
    }
}
