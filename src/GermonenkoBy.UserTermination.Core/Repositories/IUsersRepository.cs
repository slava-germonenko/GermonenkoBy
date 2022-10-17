using GermonenkoBy.UserTermination.Core.Models;

namespace GermonenkoBy.UserTermination.Core.Repositories;

public interface IUsersRepository
{
    public Task<User?> GetUserAsync(int userId);

    public Task RemoveUserAsync(int userId);
}