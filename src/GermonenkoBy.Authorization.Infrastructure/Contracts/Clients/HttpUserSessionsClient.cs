using GermonenkoBy.Authorization.Core.Contracts.Clients;
using GermonenkoBy.Authorization.Core.Dtos;
using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Authorization.Infrastructure.Contracts.Clients;

public class HttpUserSessionsClient : IUserSessionsClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpUserSessionsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<UserSession?> GetSessionAsync(Guid sessionId)
    {
        try
        {
            var response = await _httpClient.GetAsync<ContentResponse<UserSession>>($"api/user-sessions/{sessionId}");
            return response.Data;
        }
        catch (NotFoundException)
        {
            return null;
        }
    }

    public async Task<UserSession> StartUserSessionAsync(StartUserSessionDto sessionDto)
    {
        var sessionsResponse = await _httpClient.PutAsync<ContentResponse<UserSession>>(
            "api/user-sessions",
            body: sessionDto
        );
        return sessionsResponse.Data ?? throw new Exception("Произошла ошибка при попытке начать сессию.");
    }
}