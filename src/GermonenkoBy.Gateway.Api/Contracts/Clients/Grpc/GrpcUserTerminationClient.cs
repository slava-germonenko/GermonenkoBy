using UserTerminationServiceClient = GermonenkoBy.UserTermination.Api.Grpc.UserTerminationService.UserTerminationServiceClient;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Grpc;

public class GrpcUserTerminationClient : IUserTerminationClient
{
    private readonly UserTerminationServiceClient _userTerminationClient;

    public GrpcUserTerminationClient(
        UserTerminationServiceClient userTerminationClient
    )
    {
        _userTerminationClient = userTerminationClient;
    }

    public async Task TerminateAsync(int userId)
    {
        await _userTerminationClient.TerminateAsync(new()
        {
            UserId = userId,
        });
    }
}