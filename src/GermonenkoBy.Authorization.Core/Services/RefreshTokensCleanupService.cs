using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Authorization.Core.Contracts.Clients;
using GermonenkoBy.Common.HostedServices;

namespace GermonenkoBy.Authorization.Core.Services;

public class RefreshTokensCleanupService : IRunnableHostedService
{
    private readonly AuthorizationContext _context;

    private readonly IUserSessionsClient _userSessionsClient;

    private const int BatchSize = 300;

    public RefreshTokensCleanupService(AuthorizationContext context, IUserSessionsClient userSessionsClient)
    {
        _context = context;
        _userSessionsClient = userSessionsClient;
    }

    public async Task RunAsync()
    {
        var now = DateTime.UtcNow;
        var tokensToCleanUp = await _context.RefreshTokens.Where(rt => rt.ExpireDate < now)
            .OrderBy(rt => rt.ExpireDate)
            .Take(BatchSize)
            .ToListAsync();

        foreach (var token in tokensToCleanUp)
        {
            try
            {
                var session = await _userSessionsClient.GetSessionAsync(token.UserSessionId);
                if (session is not null && session.ExpireDate < now)
                {
                    await _userSessionsClient.RemoveSessionAsync(token.UserSessionId);
                }
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // TODO: Add Logging Here
                Console.WriteLine(e.Message);
            }
        }
    }
}