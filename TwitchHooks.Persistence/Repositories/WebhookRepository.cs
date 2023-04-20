using Microsoft.EntityFrameworkCore;
using TwitchHooks.Domain.Entities;
using TwitchHooks.Domain.Repositories;

namespace TwitchHooks.Infrastructure.Repositories;

public class WebhookRepository : IWebhookRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WebhookRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Webhook>> Get()
    {
        return await _dbContext.Webhooks.ToListAsync();
    }
}