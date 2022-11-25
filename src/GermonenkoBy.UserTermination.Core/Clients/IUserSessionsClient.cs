using GermonenkoBy.UserTermination.Core.Models;

namespace GermonenkoBy.UserTermination.Core.Clients;

public interface IUserSessionsClient
{
    public Task<ICollection<UserSession>> GetUserSessionsAsync(int userId);

    public Task RemoveSessionAsync(Guid sessionId);
}