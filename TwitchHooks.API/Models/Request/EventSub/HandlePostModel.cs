using System.Text.Json;
using System.Text.Json.Serialization;

namespace TwitchHooksAPI.Models.Request.EventSub;

public class HandlePostModel
{
    public string? challenge { get; set; }
    public Subscription subscription { get; set; }
    [JsonPropertyName("event")]
    public Dictionary<string, string>? Event { get; set; }

    public sealed class Subscription
    {
        public string id { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public Dictionary<string, string> condition { get; set; }
        public Transport transport { get; set; }
        public DateTime created_at { get; set; }
        public int cost { get; set; }
    }

    public sealed class Transport
    {
        public string method { get; set; }
        public string callback { get; set; }
        public string? secret { get; set; }
    }
    
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}

