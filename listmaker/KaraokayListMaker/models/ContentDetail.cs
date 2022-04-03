using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokayListMaker.models
{
    public class ContentDetail
    {
        [JsonProperty("duration")]
        public string? Duration { get; set; }
    }
}
