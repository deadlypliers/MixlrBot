using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mixlr.ApiClient.Models.Users
{
    public class User
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("about_me")]
        public string AboutMe { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("is_live")]
        public bool? IsLive { get; set; }

        [JsonProperty("broadcast_ids")]
        public List<string> BroadcastIds { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("is_premium")]
        public bool? IsPremium { get; set; }

        [JsonProperty("plan")]
        public Plan Plan { get; set; }
    }
}
