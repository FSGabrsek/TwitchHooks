using TwitchHooks.Domain.Configuration;

namespace TwitchHooks.Domain.Repositories;

public interface IClientRepository
{
    Task<string?> GetAppAccessToken(string clientID);
    
    Task<string> GetClientSecret(string clientID);

    Task<Client> SetAppAccessToken(string clientID, string appAccessToken);
}