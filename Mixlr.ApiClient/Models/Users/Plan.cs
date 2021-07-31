using Newtonsoft.Json;

namespace Mixlr.ApiClient.Models.Users
{
    public class Plan
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
