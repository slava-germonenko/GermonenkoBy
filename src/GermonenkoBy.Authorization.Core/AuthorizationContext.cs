using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Common.EntityFramework;

namespace GermonenkoBy.Authorization.Core;

public class AuthorizationContext : BaseContext
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options) { }
}