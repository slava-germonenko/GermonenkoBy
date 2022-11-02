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

    private readonly IUserSessionsClient _userSessionsClient;

    public UserAuthService(
        AccessTokenService accessTokenService,
        IAuthClient authClient,
        IUsersClient usersClient,
        IUserSessionsClient userSessionsClient
    )
    {
        _accessTokenService = accessTokenService;
        _authClient = authClient;
        _usersClient = usersClient;
        _userSessionsClient = userSessionsClient;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(AuthorizeDto authorizeDto)
    {
        var refreshToken = await _authClient.AuthorizeAsync(authorizeDto);
        var user = await _usersClient.GetUserByEmailAsync(authorizeDto.Login)
            ?? throw new CoreLogicException("Логин и/или пароль не верен.");

        var accessToken = _accessTokenService.GenerateAccessToken(user.Id);
        return new(accessToken, refreshToken, user);
    }

    public async Task<AuthorizationResult> RefreshAsync(string refreshToken)
    {
        var refreshedTokenModel = await _authClient.RefreshRefreshToken(new()
        {
            Token = refreshToken
        });

        var session = await _userSessionsClient.GetUserSessionAsync(refreshedTokenModel.UserSessionId);
        if (session is null)
        {
            throw new CoreLogicException("Сессия не действительна.");
        }

        var user = await _usersClient.GetUserAsync(session.UserId);
        if (user is null || !user.Active)
        {
            throw new CoreLogicException("Пользователь не найден.");
        }

        var accessToken = _accessTokenService.GenerateAccessToken(user.Id);
        return new(accessToken, refreshedTokenModel, user);
    }

    public Task TerminateAsync(string token) => _authClient.TerminateSessionAsync(token);
}