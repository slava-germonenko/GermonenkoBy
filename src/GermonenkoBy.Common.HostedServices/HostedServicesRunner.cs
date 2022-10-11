using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GermonenkoBy.Common.HostedServices;

public class HostedServicesRunner<TService> : BackgroundService where TService : IRunnableHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public HostedServicesRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var hostedServiceToRun = scope.ServiceProvider.GetRequiredService<TService>();

        var options = GetHostingOptions();
        if (!options.RunImmediately)
        {
            await Task.Delay(options.SleepTime, stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            options = GetHostingOptions();
            await RunSafelyAsync(hostedServiceToRun);
            await Task.Delay(options.SleepTime, stoppingToken);
        }
    }

    private async Task RunSafelyAsync(TService hostedService)
    {
        try
        {
            await hostedService.RunAsync();
        }
        catch (Exception)
        {
            // TODO: We'll need to implement logging in the future once we get to logging epic.
        }
    }

    private HostedServiceOptions<TService> GetHostingOptions()
        => _serviceProvider.GetRequiredService<HostedServiceOptions<TService>>();
}