using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.EntityFramework;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Core;

public class UsersContext : BaseContext
{
    public DbSet<User> Users => Set<User>();

    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }
}