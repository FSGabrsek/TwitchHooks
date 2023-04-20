using Microsoft.AspNetCore.Mvc;
using TwitchHooks.Application.Network;
using TwitchHooks.Domain.Repositories;
using TwitchHooksAPI.Models.Request.Subscription;

namespace TwitchHooksAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubscriptionRepository _subscriptions;
    private readonly ITwitchApiClient _twitchClient;

    public SubscriptionController(
        ILogger<SubscriptionController> logger,
        ISubscriptionRepository subscriptions, 
        ITwitchApiClient twitchClient)
    {
        _subscriptions = subscriptions;
        _twitchClient = twitchClient;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        var response = await _subscriptions.Get();
        return Results.Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetById(Guid id)
    {
        var response = await _subscriptions.GetById(id);
        return response == null ? Results.NotFound() : Results.Ok(response);
    }
    
    [HttpPost("Stream-Online")]
    public async Task<IResult> PostStreamOnline([FromBody] StreamOnlinePostModel body)
    {
        try
        {
            var eventSub = await _twitchClient.CreateStreamOnlineSubscriptionAsync(body.Subject);
            var response = new Models.Response.Subscription.StreamOnlinePostModel
            {
                twitch_subscription_id = eventSub.Id,
                status = eventSub.Status,
                created_at = eventSub.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss zzz")
            };
            return Results.Accepted("https://api.twitch.tv/helix/eventsub/subscriptions", response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create subscription");
            return Results.Problem("The server was unable to request a subscription from Twitch");
        }
    }
}