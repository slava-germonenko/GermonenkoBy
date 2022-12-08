using Grpc.Core;

using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Contacts.Api.Grpc;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Contracts.Clients.Grpc;
using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.UserTermination.Api.Grpc;

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

        var userTerminationServiceUrl = configuration.GetValueUnsafe<string>("Routing:Grpc:UserTerminationServiceUrl");
        services.AddGrpcClient<UserTerminationService.UserTerminationServiceClient>(options =>
        {
            options.Address = new Uri(userTerminationServiceUrl);
            options.ChannelOptionsActions.Add(o =>
            {
                o.Credentials = ChannelCredentials.Insecure;
            });
        });
        services.AddScoped<IUserTerminationClient, GrpcUserTerminationClient>();

        var contactsServiceUrl = configuration.GetValueUnsafe<string>("Routing:Grpc:Contacts");
        services.AddGrpcClient<ContactsService.ContactsServiceClient>(options =>
        {
            options.Address = new Uri(contactsServiceUrl);
            options.ChannelOptionsActions.Add(o =>
            {
                o.Credentials = ChannelCredentials.Insecure;
            });
        });
        services.AddScoped<IContactsClient, GrpcContactsClient>();
    }
}