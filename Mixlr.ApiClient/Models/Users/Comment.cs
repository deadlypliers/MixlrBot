using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mixlr.ApiClient.Models.Users
{
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("timestamp", ItemConverterType = typeof(UnixDateTimeConverter))]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
