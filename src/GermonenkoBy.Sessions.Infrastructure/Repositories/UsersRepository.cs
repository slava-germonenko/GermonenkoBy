using Microsoft.Extensions.Options;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Sessions.Core.Models;
using GermonenkoBy.Sessions.Core.Repositories;
using GermonenkoBy.Sessions.Infrastructure.Options;

namespace GermonenkoBy.Sessions.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly HttpClientFacade _httpClient;

    private readonly IOptionsSnapshot<RoutingOptions> _optionsSnapshot;

    private string UsersServiceHostUrl => _optionsSnapshot.Value.UsersServiceUrl;

    private const string UsersBasePath = "api/users";

    public UsersRepository(
        HttpClientFacade httpClient,
        IOptionsSnapshot<RoutingOptions> optionsSnapshot
    )
    {
        _httpClient = httpClient;
        _optionsSnapshot = optionsSnapshot;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var getUerUrl = $"{UsersServiceHostUrl}/{UsersBasePath}/{userId}";
        var user = await _httpClient.GetUnwrappedAsync<User>(getUerUrl);
        return user ?? throw new NotFoundException($"Пользователь с идентификатором \"{userId}\" не найден.");
    }
}