using Microsoft.EntityFrameworkCore;
using TwitchHooks.Domain.Entities;
using TwitchHooks.Domain.Repositories;

namespace TwitchHooks.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SubscriptionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Subscription>> Get()
    {
        return await _dbContext.Subscriptions
            .Include(s => s.Webhooks)
            .ToListAsync();
    }

    public async Task<Subscription?> GetById(Guid id)
    {
        return await _dbContext.Subscriptions
            .Include(s => s.Webhooks)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subscription> AddWebhook(Guid id, Webhook webhook)
    {
        var subscription = await GetById(id);
        if (subscription == null)
        {
            throw new Exception("Subscription not found");
        }

        subscription.Webhooks.Add(webhook);
        await _dbContext.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription> RemoveWebhook(Guid id, Webhook webhook)
    {
        var subscription = await GetById(id);
        if (subscription == null)
        {
            throw new Exception("Subscription not found");
        }

        subscription.Webhooks.Remove(webhook);
        await _dbContext.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription> Add(Subscription subscription)
    {
        await _dbContext.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription> Remove(Subscription subscription)
    {
        _dbContext.Subscriptions.Remove(subscription);
        await _dbContext.SaveChangesAsync();
        return subscription;
    }
}