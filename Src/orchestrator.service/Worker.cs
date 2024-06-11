namespace orchestrator.service;

using MassTransit;
using orchestrator.service.Extensions;

public class Worker(IBusControl bus) : BackgroundService
{
    private readonly IBusControl _bus = bus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.StartAsync(stoppingToken);

        StaticMethod.PrintMessage($"Orchestrator Service has started at: {DateTimeOffset.Now}");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bus.StopAsync(cancellationToken);

        StaticMethod.PrintMessage($"Orchestrator Service has stopped at: {DateTimeOffset.Now}");
    }
}
