using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.UserTermination.Core.Models;
using GermonenkoBy.UserTermination.Core.Repositories;

namespace GermonenkoBy.UserTermination.Infrastructure.Clients;

public class UsersClient : IUsersClient
{
    public const string ClientName = "Users";

    private readonly HttpClientFacade _httpClient;

    private const string BasesRoute = "api/users";

    public UsersClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        try
        {
            var url = GetUserUrl(userId);
            var response = await _httpClient.GetAsync<ContentResponse<User>>(url);
            return response.Data;
        }
        catch (NotFoundException)
        {
            return null;
        }
    }

    public Task RemoveUserAsync(int userId)
    {
        var url = GetUserUrl(userId);
        return _httpClient.DeleteAsync(url);
    }

    private static string GetUserUrl(int userId) => $"{BasesRoute}/{userId}";
}