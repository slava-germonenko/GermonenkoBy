using Grpc.Core;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.UserTermination.Core.Clients;
using GermonenkoBy.UserTermination.Core.Models;

namespace GermonenkoBy.UserTermination.Infrastructure.Clients;

public class GrpcUsersClient : IUsersClient
{
    private readonly UsersService.UsersServiceClient _usersService;

    public GrpcUsersClient(UsersService.UsersServiceClient usersService)
    {
        _usersService = usersService;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        try
        {
            var response = await _usersService.GetUserAsync(new()
            {
                UserId = userId,
            });
            return new()
            {
                Id = response.UserId,
            };
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task RemoveUserAsync(int userId)
    {
        await _usersService.DeleteUserAsync(new()
        {
            UserId = userId,
        });
    }
}