using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Models.Auth;

namespace GermonenkoBy.Gateway.Api.Services;

public class UserAuthService
{
    private readonly AccessTokenService _accessTokenService;

    private readonly IAuthClient _authClient;

    private readonly IUsersClient _usersClient;

    public UserAuthService(
        AccessTokenService accessTokenService,
        IAuthClient authClient,
        IUsersClient usersClient
    )
    {
        _accessTokenService = accessTokenService;
        _authClient = authClient;
        _usersClient = usersClient;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(AuthorizeDto authorizeDto)
    {
        var refreshToken = await _authClient.AuthorizeAsync(authorizeDto);
        var user = await _usersClient.GetUserByEmailAsync(authorizeDto.Login)
            ?? throw new CoreLogicException("Логин и/или пароль не верен.");

        var accessToken = _accessTokenService.GenerateAccessToken(user.Id);
        return new(accessToken, refreshToken, user);
    }
}