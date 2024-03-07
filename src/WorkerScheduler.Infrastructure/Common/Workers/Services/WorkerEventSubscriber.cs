using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Infrastructure.Common.EventBus.Services;
using WorkerScheduler.Infrastructure.Common.EventBus.Settings;

namespace WorkerScheduler.Infrastructure.Common.Workers.Services;

public class WorkerEventSubscriber(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IOptions<EventBusSubscriberSettings<WorkerEventSubscriber>> eventBusSubscriberSettings,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider
) : EventSubscriber<ProcessJobEvent, WorkerEventSubscriber>(
    rabbitMqConnectionProvider,
    eventBusSubscriberSettings,
    eventBusSubscriberSettings.Value.BusDeclarations.Select(declaration => declaration.QueueName),
    jsonSerializationSettingsProvider
)
{
    private readonly EventBusSubscriberSettings<WorkerEventSubscriber> _eventBusSubscriberSettings = eventBusSubscriberSettings.Value;

    protected override async ValueTask SetChannelAsync()
    {
        await base.SetChannelAsync();

        foreach (var busDeclaration in _eventBusSubscriberSettings.BusDeclarations)
        {
            await Channel.ExchangeDeclareAsync(busDeclaration.ExchangeName, ExchangeType.Direct, true);
            await Channel.QueueDeclareAsync(busDeclaration.QueueName, true, false, false);
            await Channel.QueueBindAsync(busDeclaration.ExchangeName, busDeclaration.QueueName, busDeclaration.BindingKey);
        }
    }

    protected override ValueTask<(bool Result, bool Redeliver)> ProcessAsync(ProcessJobEvent @event, CancellationToken cancellationToken)
    {
        return new ValueTask<(bool Result, bool Redeliver)>();
    }
}