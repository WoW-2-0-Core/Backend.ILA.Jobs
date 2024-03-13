using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Infrastructure.Common.EventBus.Services;
using WorkerScheduler.Infrastructure.Common.Schedulers.Settings;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.EventSubscribers;

/// <summary>
/// Represents scheduler event subscriber
/// </summary>
public class SchedulerEventSubscriber(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IOptions<SchedulerEventBusSettings> schedulerEventBusSettings,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider
) : EventSubscriber<RecordJobHistoryEvent, SchedulerEventSubscriber>(
    rabbitMqConnectionProvider,
    schedulerEventBusSettings,
    [schedulerEventBusSettings.Value.SchedulerIncomingBusDeclaration.QueueName],
    jsonSerializationSettingsProvider
)
{
    private readonly SchedulerEventBusSettings _schedulerEventBusSettings = schedulerEventBusSettings.Value;

    protected override async ValueTask SetChannelAsync()
    {
        await base.SetChannelAsync();

        // Declare exchange, queues and bind them
        await Channel.ExchangeDeclareAsync(_schedulerEventBusSettings.SchedulerIncomingBusDeclaration.ExchangeName, ExchangeType.Direct, true);
        await Channel.QueueDeclareAsync(_schedulerEventBusSettings.SchedulerIncomingBusDeclaration.QueueName, true, false, false);
        await Channel.QueueBindAsync(
            queue: _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.QueueName,
            exchange: _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.ExchangeName,
            routingKey: _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.RoutingKey
        );

        await Channel.ExchangeDeclareAsync(_schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.ExchangeName, ExchangeType.Direct, true);
        await Channel.QueueDeclareAsync(_schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.QueueName, true, false, false);
        await Channel.QueueBindAsync(
            queue: _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.QueueName,
            exchange: _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.ExchangeName,
            routingKey: _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.RoutingKey
        );
    }

    protected override async ValueTask<(bool Result, bool Requeue)> ProcessAsync(RecordJobHistoryEvent @event, CancellationToken cancellationToken) =>
        HandleProcessAsync is not null ? await HandleProcessAsync(@event, cancellationToken) : (false, false);

    public Func<RecordJobHistoryEvent, CancellationToken, ValueTask<(bool Result, bool Redeliver)>>? HandleProcessAsync { get; set; }
}