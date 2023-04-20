using TwitchHooks.Domain.Entities;

namespace TwitchHooks.Domain.Repositories;

public interface IWebhookRepository
{
    Task<List<Webhook>> Get();
}