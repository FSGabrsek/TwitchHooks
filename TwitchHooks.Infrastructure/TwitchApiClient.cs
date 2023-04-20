using TwitchHooks.Application.Network;
using TwitchHooks.Application.Network.Responses;
using TwitchHooks.Domain.Repositories;
using TwitchHooks.Infrastructure.Authorisation;
using TwitchHooks.Infrastructure.EventSub;

namespace TwitchHooks.Infrastructure;

public class TwitchApiClient : ITwitchApiClient
{
    private readonly IClientRepository _client;
    private readonly HttpClient _http = new();

    public TwitchApiClient(IClientRepository client)
    {
        _client = client;
    }

    public async Task<PostStreamOnlineResponse> CreateStreamOnlineSubscriptionAsync(string subjectId)
    {
        try
        {
            return await StreamOnline.Create(_client, _http, subjectId);
        }
        catch (HttpRequestException)
        {
            await ClientCredentials.AuthFlow(_client, _http);
            return await StreamOnline.Create(_client, _http, subjectId);
        }
    }

    public async Task DeleteEventSubSubscriptionAsync(string twitchSubscriptionId)
    {
        try
        {
            await EventSubscription.Delete(_client, _http, twitchSubscriptionId);
        }
        catch (HttpRequestException)
        {
            await ClientCredentials.AuthFlow(_client, _http);
            await EventSubscription.Delete(_client, _http, twitchSubscriptionId);
        }
    }
}