using Grpc.Core;

using GermonenkoBy.UserTermination.Api.Grpc;

namespace GermonenkoBy.UserTermination.Api;

public class GrpcUserTerminationService : UserTerminationService.UserTerminationServiceBase
{
    private readonly Core.UserTerminationService _userTerminationService;

    public GrpcUserTerminationService(Core.UserTerminationService userTerminationService)
    {
        _userTerminationService = userTerminationService;
    }

    public override async Task<TerminationResultResponse> Terminate(
        TerminateUserRequest request,
        ServerCallContext context
    )
    {
        await _userTerminationService.TerminateAsync(request.UserId);
        return new TerminationResultResponse();
    }
}