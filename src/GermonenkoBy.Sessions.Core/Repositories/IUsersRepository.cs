using GermonenkoBy.Sessions.Core.Models;

namespace GermonenkoBy.Sessions.Core.Repositories;

public interface IUsersRepository
{
    public Task<User> GetUserAsync(int userId);
}