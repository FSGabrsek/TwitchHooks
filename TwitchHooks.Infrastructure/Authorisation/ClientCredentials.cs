using System.Text.Json;
using TwitchHooks.Application.Network.Responses;
using TwitchHooks.Domain.Repositories;
using TwitchHooks.Infrastructure.Serialisation;

namespace TwitchHooks.Infrastructure.Authorisation;

public class ClientCredentials
{
    public static async Task AuthFlow(IClientRepository client, HttpClient http)
    {
        var clientID = Environment.GetEnvironmentVariable("CLIENT_ID");
        if (clientID == null) throw new Exception("CLIENT_ID not set");
        
        var clientSecret = await client.GetClientSecret(clientID);

        var values = new Dictionary<string, string>
        {
            {"client_id", clientID},
            {"client_secret", clientSecret},
            {"grant_type", "client_credentials"}
        };
        
        var content = new FormUrlEncodedContent(values);
        var response =  await http.PostAsync("https://id.twitch.tv/oauth2/token", content);
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                PropertyNameCaseInsensitive = true
            };
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ClientCredentialsResponse>(responseString, options);

            await client.SetAppAccessToken(clientID, responseObject!.AccessToken);
        }
        else
        {
            throw new Exception("Failed to request access token");
        }
    }
}