using Microsoft.Extensions.Options;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Infrastructure.Common.EventBus.Services;
using WorkerScheduler.Infrastructure.Common.Schedulers.EventSubscribers;
using WorkerScheduler.Infrastructure.Common.Schedulers.Settings;

namespace WorkerScheduler.Infrastructure.Common.Workers.Services;

public class WorkerEventSubscriber(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IOptions<SchedulerEventBusSettings> schedulerEventBusSettings,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider
) : EventSubscriber<ProcessJobEvent, SchedulerEventSubscriber>(
    rabbitMqConnectionProvider,
    schedulerEventBusSettings,
    [schedulerEventBusSettings.Value.SchedulerOutgoingBusDeclaration.QueueName],
    jsonSerializationSettingsProvider
)
{
    protected override ValueTask<(bool Result, bool Redeliver)> ProcessAsync(ProcessJobEvent @event, CancellationToken cancellationToken)
    {
        return new ValueTask<(bool Result, bool Redeliver)>();
    }
}