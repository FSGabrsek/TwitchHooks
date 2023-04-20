using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TwitchHooks.Domain.Entities;
using TwitchHooks.Domain.Repositories;
using TwitchHooksAPI.Models.Request.EventSub;

namespace TwitchHooksAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EventSubController : ControllerBase
{
    private const string MESSGAE_TYPE = "twitch-eventsub-message-type";
    private const string MESSAGE_TYPE_VERIFICATION = "webhook_callback_verification";
    private const string MESSAGE_TYPE_NOTIFICATION = "notification";
    private const string MESSAGE_TYPE_REVOCATION = "revocation";
    
    private const string TWITCH_MESSAGE_ID = "twitch-eventsub-message-id";
    private const string TWITCH_MESSAGE_TIMESTAMP = "twitch-eventsub-message-timestamp";
    private const string TWITCH_MESSAGE_SIGNATURE = "twitch-eventsub-message-signature";
    
    private const string HMAC_PREFIX = "sha256=";

    private readonly ISubscriptionRepository _subscriptions;

    private readonly ILogger<EventSubController> _logger;

    public EventSubController(ILogger<EventSubController> logger, ISubscriptionRepository subscriptions)
    {
        _subscriptions = subscriptions;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Handle([FromBody] HandlePostModel body)
    {
        var headers = HttpContext.Request.Headers;
        var message = headers[TWITCH_MESSAGE_ID] +
                      headers[TWITCH_MESSAGE_TIMESTAMP] +
                      body;
        
        var secret = Environment.GetEnvironmentVariable("WEBHOOK_SECRET");
        if (secret == null)
        {
            _logger.LogError("Webhook secret is not set");
            return Problem();
        }
        
        var hmac = HMAC_PREFIX + GetHmac(secret, message).ToLower();
        
        if (hmac == headers[TWITCH_MESSAGE_SIGNATURE])
        {
            return headers[MESSGAE_TYPE].ToString() switch
            {
                MESSAGE_TYPE_VERIFICATION => await HandleVerification(body),
                MESSAGE_TYPE_NOTIFICATION => await HandleNotification(),
                MESSAGE_TYPE_REVOCATION => await HandleRevocation(),
                _ => BadRequest()
            };
        }

        return Unauthorized();
    }
    
    private async Task<IActionResult> HandleVerification(HandlePostModel body)
    {
        await _subscriptions.Add(new Subscription
        {
            TwitchSubscriptionId = body.subscription.id,
            Type = body.subscription.type,
            Condition = body.subscription.condition,
        });
        
        return new OkObjectResult(body.challenge)
        {
            ContentTypes = new MediaTypeCollection {"text/plain"}
        };
    }
    
    private async Task<IActionResult> HandleNotification()
    {
        return Ok();
    }
    
    private async Task<IActionResult> HandleRevocation()
    {
        return Ok();
    }
    
    private static string GetHmac(string key, string message)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var msgBytes = Encoding.UTF8.GetBytes(message);
        using var hmac = new HMACSHA256(keyBytes);
        return Convert.ToHexString(hmac.ComputeHash(msgBytes));
    }
}