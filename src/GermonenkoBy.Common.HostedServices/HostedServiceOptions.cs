namespace GermonenkoBy.Common.HostedServices;

/// <summary>
/// Configuration for the <see cref="HostedServicesRunner{TService}"/> service runner.
/// </summary>
/// <typeparam name="TService"></typeparam>
public class HostedServiceOptions<TService> where TService : IRunnableHostedService
{
    /// <summary>
    /// If set to true, the <see cref="TService"/> will run first time just after the hosted service starts,
    /// otherwise it will wait for the <see cref="SleepTime"/> before running first time.
    /// </summary>
    public bool RunImmediately { get; set; } = true;

    /// <summary>
    /// Time between hosted service runs.
    /// </summary>
    public TimeSpan SleepTime { get; set; }
}