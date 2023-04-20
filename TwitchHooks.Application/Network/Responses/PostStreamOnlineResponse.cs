namespace TwitchHooks.Application.Network.Responses;

public class PostStreamOnlineResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string Version { get; set; }
    public Dictionary<string, string> Condition { get; set; }
    public TwitchWebhookTransport Transport { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TwitchWebhookTransport
{
    public string Method { get; set; }
    public string Callback { get; set; }
    public string Secret { get; set; }
}