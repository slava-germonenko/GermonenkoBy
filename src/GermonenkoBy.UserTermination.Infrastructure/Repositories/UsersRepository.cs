using Microsoft.Extensions.Options;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.UserTermination.Core.Models;
using GermonenkoBy.UserTermination.Core.Repositories;
using GermonenkoBy.UserTermination.Infrastructure.Options;

namespace GermonenkoBy.UserTermination.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly HttpClientFacade _httpClient;

    private readonly IOptionsSnapshot<RoutingOptions> _routingOptions;

    private string UsersServiceUrl => _routingOptions.Value.UsersServiceUrl;

    private const string BasesRoute = "api/users";

    public UsersRepository(
        HttpClientFacade httpClient,
        IOptionsSnapshot<RoutingOptions> routingOptions
    )
    {
        _httpClient = httpClient;
        _routingOptions = routingOptions;
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

    private string GetUserUrl(int userId) => $"{UsersServiceUrl}/{BasesRoute}/{userId}";
}