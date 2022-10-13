using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.EntityFramework;
using GermonenkoBy.Sessions.Core.Models;

namespace GermonenkoBy.Sessions.Core;

public class SessionsContext : BaseContext
{
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    public SessionsContext(DbContextOptions<SessionsContext> options) : base(options) { }
}