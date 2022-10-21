using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Sessions.Core.Models;
using GermonenkoBy.Sessions.Core.Repositories;

namespace GermonenkoBy.Sessions.Infrastructure.Repositories;

public class UsersClient : IUsersClient
{
    public const string ClientName = "Users";

    private readonly HttpClientFacade _httpClient;

    private const string UsersBasePath = "api/users";

    public UsersClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var user = await _httpClient.GetUnwrappedAsync<User>($"{UsersBasePath}/{userId}");
        return user ?? throw new NotFoundException($"Пользователь с идентификатором \"{userId}\" не найден.");
    }
}