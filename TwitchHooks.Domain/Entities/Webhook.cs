namespace TwitchHooks.Domain.Entities;

public class Webhook : IEntity
{
    public Guid Id { get; set; }

    public string Callback { get; set; }
    public string Message { get; set; }
}