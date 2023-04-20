namespace TwitchHooks.Application.Network.Responses;

public class ClientCredentialsResponse
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; }
}