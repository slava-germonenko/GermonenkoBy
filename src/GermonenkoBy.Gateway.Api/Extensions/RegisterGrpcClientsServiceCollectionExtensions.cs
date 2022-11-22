using GermonenkoBy.Common.Web.Extensions;
using Grpc.Core;

using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Contracts.Clients.Grpc;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Extensions;

public static class RegisterGrpcClientsServiceCollectionExtensions
{
    public static void RegisterGrpcClients(this IServiceCollection services, IConfiguration configuration)
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