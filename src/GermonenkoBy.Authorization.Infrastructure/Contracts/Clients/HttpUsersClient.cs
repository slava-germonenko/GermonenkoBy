using GermonenkoBy.Authorization.Core.Contracts.Clients;
using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Authorization.Infrastructure.Contracts.Clients;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpUsersClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<User?> GetUserAsync(string emailAddress)
    {
        var queryParams = new Dictionary<string, string?>
        {
            { "emailAddress", emailAddress },
            { "count", "1" }
        };
        var users = await _httpClient.GetAsync<ContentListResponse<User>>("api/users", queryParams);
        return users.Data?.FirstOrDefault(u => u.EmailAddress == emailAddress);
    }

    public async Task<bool> ValidatePasswordIsValid(int userId, string password)
    {
        try
        {
            var url = $"api/users/{userId}/password-validation";
            // It returns 204 if password is valid
            await _httpClient.PostAsync(url, body: new { password });
            return true;
        }
        catch (CoreLogicException)
        {
            return false;
        }
    }
}