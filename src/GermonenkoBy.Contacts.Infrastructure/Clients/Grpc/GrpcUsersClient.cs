using Grpc.Core;

using GermonenkoBy.Contacts.Core.Models;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Contacts.Infrastructure.Clients.Grpc;

public class GrpcUsersClient : IUsersClient
{
    private readonly UsersService.UsersServiceClient _grpcUsersClient;

    public GrpcUsersClient(UsersService.UsersServiceClient grpcUsersClient)
    {
        _grpcUsersClient = grpcUsersClient;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        try
        {
            var response = await _grpcUsersClient.GetUserAsync(new()
            {
                UserId = userId,
            });

            return new User
            {
                Id = response.UserId,
            };
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }
}