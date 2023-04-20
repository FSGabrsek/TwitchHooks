namespace TwitchHooks.Domain.Entities;

public class Subscription : IEntity
{
    public Guid Id { get; set; }

    public string TwitchSubscriptionId { get; set; }
    public string Type { get; set; }
    public Dictionary<string, string> Condition { get; set; }
    public List<Webhook> Webhooks { get; set; }
}