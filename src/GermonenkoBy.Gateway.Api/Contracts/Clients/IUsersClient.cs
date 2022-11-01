using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IUsersClient
{
    public Task<PagedSet<User>> GetUsersAsync(UsersFilterDto usersFilter);
}