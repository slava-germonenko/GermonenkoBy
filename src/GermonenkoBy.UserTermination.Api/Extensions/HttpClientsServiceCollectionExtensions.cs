using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.UserTermination.Core.Clients;
using GermonenkoBy.UserTermination.Infrastructure.Clients;

namespace GermonenkoBy.UserTermination.Api.Extensions;

public static class HttpClientsServiceCollectionExtensions
{
    public static void RegisterHttpClients(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var sessionsServiceBaseAddress = configuration.GetValueUnsafe<string>("Routing:Http:SessionsServiceUrl");
        services.AddHttpClient<IUserSessionsClient, UserSessionsClient>(
            UserSessionsClient.ClientName,
            options =>
            {
                options.BaseAddress = new Uri(sessionsServiceBaseAddress);
            }
        );
    }
}