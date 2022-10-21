using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Sessions.Core.Dtos;
using GermonenkoBy.Sessions.Core.Models;
using GermonenkoBy.Sessions.Core.Repositories;

namespace GermonenkoBy.Sessions.Core.Services;

public class UserSessionsService
{
    private readonly IUsersClient _client;

    private readonly SessionsContext _context;

    public UserSessionsService(IUsersClient client, SessionsContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<UserSession> StartOrRefreshSessionAsync(StartUserSessionDto sessionDto)
    {
        var session = await _context.UserSessions.FirstOrDefaultAsync(
            s => s.DeviceId == sessionDto.DeviceId && s.UserId == sessionDto.UserId
        );

        if (session is null)
        {
            await EnsureUserExistsAsync(sessionDto.UserId);
            session = new()
            {
                DeviceId = sessionDto.DeviceId,
                DeviceName = sessionDto.DeviceName,
                UserId = sessionDto.UserId
            };
        }

        if (session.ExpireDate > sessionDto.ExpireDate)
        {
            throw new CoreLogicException("Невозможно сократить время уже существующей действия сессии.");
        }
        session.ExpireDate = sessionDto.ExpireDate;

        _context.UserSessions.Update(session);
        await _context.SaveChangesAsync();

        return session;
    }

    public async Task TerminateSessionAsync(Guid sessionId)
    {
        var session = await _context.UserSessions.FindAsync(sessionId);
        if (session is not null)
        {
            _context.UserSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }

    private async Task EnsureUserExistsAsync(int userId)
    {
        try
        {
            await _client.GetUserAsync(userId);
        }
        catch (NotFoundException e)
        {
            throw new CoreLogicException(e.Message);
        }
    }
}