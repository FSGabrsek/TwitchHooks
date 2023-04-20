using System.Net.Http.Headers;
using TwitchHooks.Domain.Repositories;

namespace TwitchHooks.Infrastructure.EventSub;

public class EventSubscription
{
    public static async Task Delete(IClientRepository client, HttpClient http, string eventSubId)
    {
        var clientID = Environment.GetEnvironmentVariable("CLIENT_ID");
        if (clientID == null) throw new Exception("CLIENT_ID not set");

        var token = await client.GetAppAccessToken(clientID);
        
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await http.DeleteAsync("https://api.twitch.tv/helix/eventsub/subscriptions?id=" + eventSubId);
        
        if (!response.IsSuccessStatusCode) throw new HttpRequestException("Failed to delete subscription");
    }
}