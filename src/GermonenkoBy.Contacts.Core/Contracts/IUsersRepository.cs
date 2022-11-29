using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Core.Contracts;

public interface IUsersRepository
{
    public Task<User?> GetUserAsync(int id);
}