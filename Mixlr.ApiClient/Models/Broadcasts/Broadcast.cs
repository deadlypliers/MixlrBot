using Mixlr.ApiClient.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mixlr.ApiClient.Models.Broadcasts
{
    public class Broadcast
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("category_id")]
        public int? CategoryId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("comment_count")]
        public int? CommentCount { get; set; }

        [JsonProperty("listener_count")]
        public int? ListenerCount { get; set; }

        [JsonProperty("heart_count")]
        public int? HeartCount { get; set; }

        [JsonProperty("seconds_since_start")]
        public long? SecondsSinceStart { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("tag_list")]
        public List<string> TagList { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("is_live")]
        public bool? IsLive { get; set; }

        [JsonProperty("started_at")]
        public DateTime? StartedAt { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("auto_start_events")]
        public bool? AutoStartEvents { get; set; }
    }
}
