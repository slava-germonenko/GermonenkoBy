using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Extensions;

public static class UsersClientExtensions
{
    public static async Task<User?> GetUserByEmailAsync(this IUsersClient usersClient, string emailAddress)
    {
        var filet = new UsersFilterDto
        {
            EmailAddress = emailAddress,
            Count = 1,
        };
        var result = await usersClient.GetUsersAsync(filet);
        return result.Data.FirstOrDefault();
    }
}