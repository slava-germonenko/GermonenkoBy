using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.HostedServices;

namespace GermonenkoBy.Sessions.Core.Services;

public class UserSessionsCleanupService : IRunnableHostedService
{
    private readonly SessionsContext _context;

    private const int SessionsCleanupBatchSize = 100;

    public UserSessionsCleanupService(SessionsContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        var sessionCountToCleanup = await _context.UserSessions.CountAsync(
            session => session.ExpireDate <= DateTime.UtcNow
        );

        for (int offset = 0; offset < sessionCountToCleanup; offset += SessionsCleanupBatchSize)
        {
            var sessionsToCleanUp = await _context.UserSessions
                .AsTracking()
                .Where(session => session.ExpireDate <= DateTime.UtcNow)
                .ToListAsync();

            _context.UserSessions.RemoveRange(sessionsToCleanUp);
            await _context.SaveChangesAsync();
        }
    }
}