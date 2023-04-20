using Microsoft.EntityFrameworkCore;
using TwitchHooks.Domain.Configuration;
using TwitchHooks.Domain.Repositories;

namespace TwitchHooks.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ClientRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string?> GetAppAccessToken(string clientID)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientID == clientID);
        return client?.AppAccessToken;
    }

    public async Task<string> GetClientSecret(string clientID)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientID == clientID);
        return client!.ClientSecret;
    }

    public async Task<Client> SetAppAccessToken(string clientID, string appAccessToken)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientID == clientID);
        if (client == null)
        {
            throw new Exception("Client not found");
        }

        client.AppAccessToken = appAccessToken;
        await _dbContext.SaveChangesAsync();
        return client;
    }
}