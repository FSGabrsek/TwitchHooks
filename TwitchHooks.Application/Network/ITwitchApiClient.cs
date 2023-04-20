using TwitchHooks.Application.Network.Responses;

namespace TwitchHooks.Application.Network;

public interface ITwitchApiClient
{
    Task<PostStreamOnlineResponse> CreateStreamOnlineSubscriptionAsync(string subjectId);
    Task DeleteEventSubSubscriptionAsync(string twitchSubscriptionId);

}