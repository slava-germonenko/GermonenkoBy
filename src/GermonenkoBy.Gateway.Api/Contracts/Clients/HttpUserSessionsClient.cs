using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Models.Sessions;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public class HttpUserSessionsClient : IUserSessionsClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpUserSessionsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<UserSession?> GetUserSessionAsync(Guid userSessionId)
    {
        var url = $"api/user-sessions/{userSessionId}";
        try
        {
            var response = await _httpClient.GetAsync<ContentResponse<UserSession>>(url);
            return response.Data;
        }
        catch (NotFoundException)
        {
            return null;
        }
    }
}