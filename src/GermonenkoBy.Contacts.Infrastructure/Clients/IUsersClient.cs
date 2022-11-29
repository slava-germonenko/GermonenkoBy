using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Infrastructure.Clients;

public interface IUsersClient
{
    public Task<User?> GetUserAsync(int userId);
}