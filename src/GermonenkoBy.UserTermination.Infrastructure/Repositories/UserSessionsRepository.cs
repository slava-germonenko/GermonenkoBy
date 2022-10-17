using Microsoft.Extensions.Options;

using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.UserTermination.Core.Models;
using GermonenkoBy.UserTermination.Core.Repositories;
using GermonenkoBy.UserTermination.Infrastructure.Options;

namespace GermonenkoBy.UserTermination.Infrastructure.Repositories;

public class UserSessionsRepository : IUserSessionsRepository
{
    private readonly HttpClientFacade _httpClient;

    private readonly IOptionsSnapshot<RoutingOptions> _routingOptions;

    private string SessionsServiceUrl => _routingOptions.Value.SessionsServiceUrl;

    private const string BaseRoute = "api/user-sessions";

    public UserSessionsRepository(
        HttpClientFacade httpClient,
        IOptionsSnapshot<RoutingOptions> routingOptions
    )
    {
        _httpClient = httpClient;
        _routingOptions = routingOptions;
    }

    public async Task<ICollection<UserSession>> GetUserSessionsAsync(int userId)
    {
        var url = $"{SessionsServiceUrl}/{BaseRoute}";
        var queryParams = new Dictionary<string, string?>
        {
            {"userId", userId.ToString()},
            {"count", int.MaxValue.ToString()}
        };
        var userSessionsPage = await _httpClient.GetAsync<ContentListResponse<UserSession>>(url, queryParams);
        return userSessionsPage.Data ?? new List<UserSession>();
    }

    public Task RemoveSessionAsync(Guid sessionId)
    {
        var url = $"{SessionsServiceUrl}/{BaseRoute}/{sessionId}";
        return _httpClient.DeleteAsync(url);
    }
}