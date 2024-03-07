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
) : EventSubscriber<ProcessJobEvent, SchedulerEventSubscriber>(
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
            _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.ExchangeName,
            _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.QueueName,
            _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.BindingKey
        );
        
        await Channel.ExchangeDeclareAsync(_schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.ExchangeName, ExchangeType.Direct, true);
        await Channel.QueueDeclareAsync(_schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.QueueName, true, false, false);
        await Channel.QueueBindAsync(
            _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.ExchangeName,
            _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.QueueName,
            _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.BindingKey
        );
    }

    protected override ValueTask<(bool Result, bool Redeliver)> ProcessAsync(ProcessJobEvent @event, CancellationToken cancellationToken)
    {
        return new ValueTask<(bool Result, bool Redeliver)>();
    }
}