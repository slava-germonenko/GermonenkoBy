using GermonenkoBy.Contacts.Core.Contracts;
using GermonenkoBy.Contacts.Core.Models;
using GermonenkoBy.Contacts.Infrastructure.Clients;

namespace GermonenkoBy.Contacts.Infrastructure.Repos;

public class UsersRepository : IUsersRepository
{
    private readonly IUsersClient _usersClient;

    public UsersRepository(IUsersClient usersClient)
    {
        _usersClient = usersClient;
    }

    public Task<User?> GetUserAsync(int id) => _usersClient.GetUserAsync(id);
}