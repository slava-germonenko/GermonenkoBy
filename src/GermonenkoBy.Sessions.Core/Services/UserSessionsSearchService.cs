using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Sessions.Core.Dtos;
using GermonenkoBy.Sessions.Core.Models;

namespace GermonenkoBy.Sessions.Core.Services;

public class UserSessionsSearchService
{
    private readonly SessionsContext _context;

    public UserSessionsSearchService(SessionsContext context)
    {
        _context = context;
    }

    public async Task<PagedSet<UserSession>> GetUserSessionsAsync(FilterUserSessionsDto sessionsFilter)
    {
        var query = _context.UserSessions.AsNoTracking();

        if (sessionsFilter.UserId is not null)
        {
            query = query.Where(session => session.UserId == sessionsFilter.UserId);
        }

        if (sessionsFilter.DeviceId is not null)
        {
            query = query.Where(session => session.DeviceId == sessionsFilter.DeviceId);
        }

        return await query.OrderByDescending(session => session.ExpireDate).ToPagedSetAsync(sessionsFilter);
    }
}