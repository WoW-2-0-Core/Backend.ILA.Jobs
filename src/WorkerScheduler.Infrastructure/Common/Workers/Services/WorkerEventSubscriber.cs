using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Application.Common.Workers.Services;
using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Extensions;
using WorkerScheduler.Infrastructure.Common.EventBus.Services;
using WorkerScheduler.Infrastructure.Common.Schedulers.Settings;
using WorkerScheduler.Infrastructure.Common.Workers.Settings;

namespace WorkerScheduler.Infrastructure.Common.Workers.Services;

public class WorkerEventSubscriber(
    IEventBusBroker eventBusBroker,
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IOptions<SchedulerEventBusSettings> schedulerEventBusSettings,
    IOptions<WorkerEventBusSettings> workerEventBusSettings,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider,
    IServiceScopeFactory serviceScopeFactory
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
    
    protected override async ValueTask<(bool Result, bool Requeue)> ProcessAsync(ProcessJobEvent @event, CancellationToken cancellationToken)
    {
        var scopedServiceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

        // Execute with try-catch
        var executeJobTask = async () =>
        {
            var executor = scopedServiceProvider.GetRequiredKeyedService<ITaskExecutor>(@event.Job.JobType);
            return await executor.ExecuteAsync(@event.Job);
        };

        var executeJobResult = await executeJobTask.GetValueAsync();

        var recordJobHistoryEvent = new RecordJobHistoryEvent
        {
            JobId = @event.Job.Id,
            ParentHistoryId = @event.ParentHistoryId,
            IsSuccessful = executeJobResult.IsSuccess,
            RetryCount = @event.ParentHistoryId.HasValue ? (byte)(@event.RetryCount + 1) : default
        };

        await eventBusBroker.PublishAsync(
            recordJobHistoryEvent,
            new EventOptions
            {
                Exchange = _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.ExchangeName,
                RoutingKey = _schedulerEventBusSettings.SchedulerIncomingBusDeclaration.RoutingKey,
                CorrelationId = recordJobHistoryEvent.Id.ToString()
            },
            cancellationToken
        );
        
        return (true, false);
    }
}