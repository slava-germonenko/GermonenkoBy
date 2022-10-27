using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Authorization.Core.Contracts;
using GermonenkoBy.Authorization.Core.Contracts.Clients;
using GermonenkoBy.Authorization.Core.Dtos;
using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Common.Domain.Exceptions;

namespace GermonenkoBy.Authorization.Core.Services;

public class DefaultUserAuthorizationService
{
    private readonly AuthorizationContext _context;

    private readonly IExpireDateGenerator _expireDateGenerator;

    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    private readonly IUsersClient _usersClient;

    private readonly IUserSessionsClient _sessionsClient;

    private readonly TimeSpan _defaultSessionExtensionTime = TimeSpan.FromMinutes(5);

    private const string DefaultAuthErrorMessage = "Логин и/или пароль не верны.";

    public DefaultUserAuthorizationService(
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

    public async Task<RefreshToken> RefreshRefreshTokenAsync(RefreshRefreshTokenDto refreshRefreshTokenDto)
    {
        var now = DateTime.UtcNow;
        var refreshToken = await _context.RefreshTokens.FindAsync(refreshRefreshTokenDto.Token);
        if (refreshToken is null || refreshToken.ExpireDate < now)
        {
            throw new CoreLogicException("Сессия не действительная.");
        }

        var session = await _sessionsClient.GetSessionAsync(refreshToken.UserSessionId);
        if (session is null || session.ExpireDate < now)
        {
            throw new CoreLogicException("Данный токен не действетелен.");
        }

        if (refreshRefreshTokenDto.ExpireDate is not null)
        {
            session.ExpireDate = refreshRefreshTokenDto.ExpireDate.Value;
        }
        else
        {
            var tokenSoonToExpire = (now - session.ExpireDate).Duration() < _defaultSessionExtensionTime;
            // Slightly extend expire date so the session is not terminated suddenly for the user.
            if (tokenSoonToExpire)
            {
                session.ExpireDate = now.Add(_defaultSessionExtensionTime);
            }
        }

        await _sessionsClient.StartUserSessionAsync(new()
        {
            UserId = session.UserId,
            DeviceId = session.DeviceId,
            DeviceName = session.DeviceName,
            ExpireDate = session.ExpireDate
        });

        await RemoveSessionRelatedRefreshTokenAsync(session.Id);

        var newRefreshToken = new RefreshToken
        {
            UserSessionId = session.Id,
            ExpireDate = session.ExpireDate,
            Token = _refreshTokenGenerator.GenerateRefreshToken()
        };
        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return newRefreshToken;
    }

    public async Task TerminateSessionAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FindAsync(token);
        if (refreshToken is null)
        {
            return;
        }

        await RemoveSessionRelatedRefreshTokenAsync(refreshToken.UserSessionId);
        await _context.SaveChangesAsync();
        await _sessionsClient.RemoveSessionAsync(refreshToken.UserSessionId);
    }

    private async Task RemoveSessionRelatedRefreshTokenAsync(Guid sessionId)
    {
        var refreshToken = await _context.RefreshTokens
            .Where(rf => rf.UserSessionId == sessionId)
            .ToListAsync();

        if (refreshToken.Any())
        {
            _context.RefreshTokens.RemoveRange(refreshToken);
        }
    }
}