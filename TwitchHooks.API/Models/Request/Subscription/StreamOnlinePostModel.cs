using System.ComponentModel.DataAnnotations;

namespace TwitchHooksAPI.Models.Request.Subscription;

public class StreamOnlinePostModel
{
    [Required]
    public string Subject { get; set; }
}