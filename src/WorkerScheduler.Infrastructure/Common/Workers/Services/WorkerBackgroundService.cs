using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerScheduler.Application.Common.EventBus.EventSubscribers;
using WorkerScheduler.Domain.Constants;

namespace WorkerScheduler.Infrastructure.Common.Workers.Services;

public class WorkerBackgroundService([FromKeyedServices(EventBusConstants.WorkerEventSubscriber)] IEnumerable<IEventSubscriber> eventSubscribers)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.WhenAll(eventSubscribers.Select(eventSubscriber => eventSubscriber.StartAsync(stoppingToken).AsTask()));
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(eventSubscribers.Select(eventSubscriber => eventSubscriber.StopAsync(cancellationToken).AsTask()));
    }
}