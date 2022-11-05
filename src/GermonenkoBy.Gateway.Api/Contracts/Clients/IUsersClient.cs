using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IUsersClient
{
    public Task<User?> GetUserAsync(int userId);

    public Task<PagedSet<User>> GetUsersAsync(UsersFilterDto usersFilter);

    public Task<User> CreateUserAsync(CreateUserDto userDto);

    public Task<User> UpdateUserAsync(int userId, ModifyUserDto userDto);

    Task SetUserPasswordAsync(int userId, string password);

    public Task DeleteUserAsync(int userId);
}