using GermonenkoBy.UserTermination.Core.Models;

namespace GermonenkoBy.UserTermination.Core.Repositories;

public interface IUserSessionsRepository
{
    public Task<ICollection<UserSession>> GetUserSessionsAsync(int userId);

    public Task RemoveSessionAsync(Guid sessionId);
}