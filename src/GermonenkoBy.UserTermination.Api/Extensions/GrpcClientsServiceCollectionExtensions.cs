using Grpc.Core;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.UserTermination.Core.Clients;
using GermonenkoBy.UserTermination.Infrastructure.Clients;
using GermonenkoBy.Common.Web.Extensions;

namespace GermonenkoBy.UserTermination.Api.Extensions;

public static class GrpcClientsServiceCollectionExtensions
{
    public static void RegisterGrpcClients(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var usersServiceUrl = configuration.GetValueUnsafe<string>("Routing:Grpc:UsersServiceUrl");
        services.AddGrpcClient<UsersService.UsersServiceClient>(options =>
        {
            options.Address = new Uri(usersServiceUrl);
            options.ChannelOptionsActions.Add(o =>
            {
                o.Credentials = ChannelCredentials.Insecure;
            });
        });
        services.AddScoped<IUsersClient, GrpcUsersClient>();
    }
}
