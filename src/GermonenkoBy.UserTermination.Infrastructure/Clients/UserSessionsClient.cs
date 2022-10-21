using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.UserTermination.Core.Models;
using GermonenkoBy.UserTermination.Core.Repositories;

namespace GermonenkoBy.UserTermination.Infrastructure.Clients;

public class UserSessionsClient : IUserSessionsClient
{
    public const string ClientName = "UserSessions";

    private readonly HttpClientFacade _httpClient;

    private const string BaseRoute = "api/user-sessions";

    public UserSessionsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<ICollection<UserSession>> GetUserSessionsAsync(int userId)
    {
        var queryParams = new Dictionary<string, string?>
        {
            {"userId", userId.ToString()},
            {"count", int.MaxValue.ToString()}
        };
        var userSessionsPage = await _httpClient.GetAsync<ContentListResponse<UserSession>>(BaseRoute, queryParams);
        return userSessionsPage.Data ?? new List<UserSession>();
    }

    public Task RemoveSessionAsync(Guid sessionId)
    {
        var url = $"{BaseRoute}/{sessionId}";
        return _httpClient.DeleteAsync(url);
    }
}