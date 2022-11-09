using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Models.Auth;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpAuthClient : IAuthClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpAuthClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<RefreshToken> AuthorizeAsync(AuthorizeDto authorizeDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<RefreshToken>>("api/users-auth", body: authorizeDto);
        return response.Data ?? throw new Exception("Произошла ошибка при попытке авторизации.");
    }

    public async Task<RefreshToken> RefreshRefreshToken(RefreshRefreshTokenDto refreshTokenDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<RefreshToken>>(
            "api/users-auth/refresh",
            body: refreshTokenDto
        );
        return response.Data ?? throw new Exception("Произошла ошибка при попытке авторизации.");
    }

    public Task TerminateSessionAsync(string token)
    {
        return _httpClient.PostAsync("api/users-auth/terminate", body: new { token });
    }
}