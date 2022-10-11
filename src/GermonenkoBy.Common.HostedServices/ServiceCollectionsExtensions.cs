using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GermonenkoBy.Common.HostedServices;

public static class ServiceCollectionsExtensions
{
    public static void RegisterHostedService<TService>(
        this IServiceCollection services,
        TimeSpan sleepTime
    ) where TService : class, IRunnableHostedService
    {
        var options = new HostedServiceOptions<TService>
        {
            SleepTime = sleepTime,
        };
        services.RegisterHostedService(options);
    }

    public static void RegisterHostedService<TService>(
        this IServiceCollection services,
        HostedServiceOptions<TService> options
    ) where TService : class, IRunnableHostedService
    {
        var optionsServiceDescriptor = new ServiceDescriptor(
            typeof(HostedServiceOptions<TService>),
            options
        );

        services.TryAdd(optionsServiceDescriptor);

        services.TryAddScoped<TService>();

        services.AddHostedService<HostedServicesRunner<TService>>();
    }
}