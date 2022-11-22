using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

namespace GermonenkoBy.Gateway.Api.Extensions;

public static class RegisterHttpClientsServiceCollectionExtensions
{
    public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var authServiceUrl = configuration.GetValueUnsafe<string>("Routing:Http:AuthorizationServiceUrl");
        services.AddHttpClient<IAuthClient, HttpAuthClient>(options =>
        {
            options.BaseAddress = new Uri(authServiceUrl);
        });

        var usersServiceUrl = configuration.GetValueUnsafe<string>("Routing:Http:UsersServiceUrl");
        services.AddHttpClient<IUsersClient, HttpUsersClient>(options =>
        {
            options.BaseAddress = new Uri(usersServiceUrl);
        });

        var sessionServiceUrl = configuration.GetValueUnsafe<string>("Routing:Http:SessionsServiceUrl");
        services.AddHttpClient<IUserSessionsClient, HttpUserSessionsClient>(options =>
        {
            options.BaseAddress = new Uri(sessionServiceUrl);
        });

        var productsServiceUrl = configuration.GetValueUnsafe<string>("Routing:Http:ProductsServiceUrl");
        services.AddHttpClient<IMaterialsClient, HttpMaterialsClient>(options =>
        {
            options.BaseAddress = new Uri(productsServiceUrl);
        });
        services.AddHttpClient<ICategoriesClient, HttpCategoriesClient>(options =>
        {
            options.BaseAddress = new Uri(productsServiceUrl);
        });
        services.AddHttpClient<IProductsClient, HttpProductsClient>(options =>
        {
            options.BaseAddress = new Uri(productsServiceUrl);
        });
        services.AddHttpClient<IProductAssetsClient, HttpProductAssetsClient>(options =>
        {
            options.BaseAddress = new Uri(productsServiceUrl);
        });

    }
}