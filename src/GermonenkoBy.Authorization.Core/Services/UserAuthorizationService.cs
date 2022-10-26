using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Authorization.Core.Contracts;
using GermonenkoBy.Authorization.Core.Contracts.Clients;
using GermonenkoBy.Authorization.Core.Dtos;
using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Common.Domain.Exceptions;

namespace GermonenkoBy.Authorization.Core.Services;

public class AuthorizationService
{
    private readonly AuthorizationContext _context;

    private readonly IExpireDateGenerator _expireDateGenerator;

    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    private readonly IUsersClient _usersClient;

    private readonly IUserSessionsClient _sessionsClient;

    private const string DefaultAuthErrorMessage = "Логин и/или пароль не верны.";

    public AuthorizationService(
        AuthorizationContext context,
        IExpireDateGenerator expireDateGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUserSessionsClient sessionsClient,
        IUsersClient usersClient
    )
    {
        _context = context;
        _expireDateGenerator = expireDateGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _sessionsClient = sessionsClient;
        _usersClient = usersClient;
    }

    public async Task<RefreshToken> AuthorizeAsync(AuthorizeDto authorizeDto)
    {
        var user = await _usersClient.GetUserAsync(authorizeDto.Login);
        if (user is null)
        {
            throw new CoreLogicException(DefaultAuthErrorMessage);
        }

        var passwordIsValid = await _usersClient.ValidatePasswordIsValid(user.Id, authorizeDto.Password);
        if (!passwordIsValid)
        {
            throw new CoreLogicException(DefaultAuthErrorMessage);
        }

        var expireDate = _expireDateGenerator.GenerateSessionExpireDate();
        var sessionDto = new StartUserSessionDto
        {
            UserId = user.Id,
            DeviceName = authorizeDto.DeviceName,
            DeviceId = authorizeDto.DeviceId,
            ExpireDate = expireDate,
        };
        var session = await _sessionsClient.StartUserSessionAsync(sessionDto);
        var refreshToken = new RefreshToken
        {
            UserSessionId = session.Id,
            ExpireDate = expireDate,
            Token = _refreshTokenGenerator.GenerateRefreshToken()
        };

        await RemoveSessionRelatedRefreshTokenAsync(session.Id);
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    private async Task RemoveSessionRelatedRefreshTokenAsync(Guid sessionId)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(
            rf => rf.UserSessionId == sessionId
        );

        if (refreshToken is not null)
        {
            _context.RefreshTokens.Remove(refreshToken);
        }
    }
}