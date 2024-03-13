using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Infrastructure.Common.EventBus.Services;
using WorkerScheduler.Infrastructure.Common.Schedulers.EventSubscribers;
using WorkerScheduler.Infrastructure.Common.Schedulers.Settings;
using WorkerScheduler.Infrastructure.Common.Workers.Settings;

namespace WorkerScheduler.Infrastructure.Common.Workers.Services;

public class WorkerEventSubscriber(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IOptions<SchedulerEventBusSettings> schedulerEventBusSettings,
    IOptions<WorkerEventBusSettings> workerEventBusSettings,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider,
) : EventSubscriber<ProcessJobEvent, WorkerEventSubscriber>(
    rabbitMqConnectionProvider,
    workerEventBusSettings,
    [schedulerEventBusSettings.Value.SchedulerOutgoingBusDeclaration.QueueName],
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
            _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.QueueName,
            _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.ExchangeName,
            _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.BindingKey
        );
    }
    
    protected override ValueTask<(bool Result, bool Redeliver)> ProcessAsync(ProcessJobEvent @event, CancellationToken cancellationToken)
    {
        return new ValueTask<(bool Result, bool Redeliver)>();
    }
}