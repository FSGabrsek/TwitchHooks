using TwitchHooks.Domain.Entities;

namespace TwitchHooks.Domain.Repositories;

public interface ISubscriptionRepository
{
    Task<List<Subscription>> Get();
    Task<Subscription?> GetById(Guid id);
    
    Task<Subscription> AddWebhook(Guid id, Webhook webhook);
    
    Task<Subscription> RemoveWebhook(Guid id, Webhook webhook);

    Task<Subscription> Add(Subscription subscription);
    
    Task<Subscription> Remove(Subscription subscription);
}