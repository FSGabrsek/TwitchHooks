namespace TwitchHooksAPI.Models.Response.Subscription;

public class StreamOnlinePostModel
{
    public string twitch_subscription_id { get; set; }
    public string status { get; set; }
    public string created_at { get; set; }
}