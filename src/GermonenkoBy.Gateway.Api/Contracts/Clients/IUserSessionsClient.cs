using GermonenkoBy.Gateway.Api.Models.Sessions;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IUserSessionsClient
{
    public Task<UserSession?> GetUserSessionAsync(Guid userSessionId);
}