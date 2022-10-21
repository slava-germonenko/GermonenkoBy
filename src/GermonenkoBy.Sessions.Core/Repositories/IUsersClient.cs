using GermonenkoBy.Sessions.Core.Models;

namespace GermonenkoBy.Sessions.Core.Repositories;

public interface IUsersClient
{
    public Task<User> GetUserAsync(int userId);
}