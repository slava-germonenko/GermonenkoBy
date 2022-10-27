using GermonenkoBy.Authorization.Core.Dtos;
using GermonenkoBy.Authorization.Core.Models;

namespace GermonenkoBy.Authorization.Core.Contracts.Clients;

public interface IUserSessionsClient
{
    public Task<UserSessions> StartUserSessionAsync(StartUserSessionDto sessionDto);
}