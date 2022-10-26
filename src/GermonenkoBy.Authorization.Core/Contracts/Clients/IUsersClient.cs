using GermonenkoBy.Authorization.Core.Models;

namespace GermonenkoBy.Authorization.Core.Contracts.Clients;

public interface IUsersClient
{
    public Task<User?> GetUserAsync(string emailAddress);

    public Task<bool> ValidatePasswordIsValid(int userId, string password);
}