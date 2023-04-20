using System.Net.Http.Headers;
using System.Text.Json;
using TwitchHooks.Application.Network.Responses;
using TwitchHooks.Domain.Repositories;
using TwitchHooks.Infrastructure.Serialisation;

namespace TwitchHooks.Infrastructure.EventSub;

public abstract class StreamOnline
{
    public static async Task<PostStreamOnlineResponse> Create(IClientRepository client, HttpClient http, string subjectID)
    {
        var clientID = Environment.GetEnvironmentVariable("CLIENT_ID");
        if (clientID == null) throw new Exception("CLIENT_ID not set");

        var token = await client.GetAppAccessToken(clientID);

        var values = new Dictionary<string, string>
        {
            {"type", "stream.online"},
            {"version", "1"},
            {"condition", "{\"broadcaster_user_id\":\"" + subjectID + "\"}"},
            {
                "transport",
                "{\"method\":\"webhook\",\"callback\":\"https://twitchhooks.azurewebsites.net/subscription/stream-online\",\"secret\":\"" +
                Environment.GetEnvironmentVariable("WEBHOOK_SECRET") + "\"}"
            }
        };
        var content = new FormUrlEncodedContent(values);
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await http.PostAsync("https://api.twitch.tv/helix/eventsub/subscriptions", content);

        if (!response.IsSuccessStatusCode) throw new HttpRequestException("Failed to create subscription");
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            PropertyNameCaseInsensitive = true
        };
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<PostStreamOnlineResponse>(responseString, options);
        return responseObject!;
    }
}