using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mixlr.ApiClient.Models.Users
{
    public class BroadcastAction
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("timestamp", ItemConverterType = typeof(UnixDateTimeConverter))]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("type", ItemConverterType = typeof(StringEnumConverter))]
        public BroadcastActionType? Type { get; set; }
    }
}
