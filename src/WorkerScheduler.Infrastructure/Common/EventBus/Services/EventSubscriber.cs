using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.EventBus.EventSubscribers;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Common.Exceptions;
using WorkerScheduler.Domain.Extensions;
using WorkerScheduler.Infrastructure.Common.EventBus.Settings;

namespace WorkerScheduler.Infrastructure.Common.EventBus.Services;

public abstract class EventSubscriber<TEvent, TEventSubscriber> : IEventSubscriber 
    where TEvent : Event 
    where TEventSubscriber : IEventSubscriber
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly IRabbitMqConnectionProvider _rabbitMqConnectionProvider;
    private readonly EventBusSubscriberSettings<TEventSubscriber> _eventBusSubscriberSettings;
    private readonly IEnumerable<string> _queueNames;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private IEnumerable<EventingBasicConsumer> _consumers = default!;
    protected IChannel Channel = default!;

    public EventSubscriber(IRabbitMqConnectionProvider rabbitMqConnectionProvider,
        IOptions<EventBusSubscriberSettings<TEventSubscriber>> schedulerEventBusSettings,
        IEnumerable<string> queueNames,
        IJsonSerializationSettingsProvider jsonSerializationSettingsProvider)
    {
        _rabbitMqConnectionProvider = rabbitMqConnectionProvider;
        _eventBusSubscriberSettings = schedulerEventBusSettings.Value;
        _queueNames = queueNames;

        _jsonSerializerSettings = jsonSerializationSettingsProvider.Get(true);
        _jsonSerializerSettings.ContractResolver = new DefaultContractResolver();
        _jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
    }

    public async ValueTask StartAsync(CancellationToken token)
    {
        await SetChannelAsync();
        await SetConsumerAsync(_cancellationTokenSource.Token);
    }

    
    public ValueTask StopAsync(CancellationToken token)
    {
        _cancellationTokenSource.Cancel();
        Channel.Dispose();
        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetChannelAsync()
    {
        Channel = await _rabbitMqConnectionProvider.CreateChannelAsync();
        await Channel.BasicQosAsync(0, _eventBusSubscriberSettings.PrefetchCount, false);
    }

    protected virtual async ValueTask SetConsumerAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;
        
        _consumers = await Task.WhenAll(
            _queueNames.Select(
                async queueName =>
                {
                    var consumer = new EventingBasicConsumer(Channel);
                    consumer.Received += async (sender, args) => await HandleInternalAsync(sender, args, cancellationToken);
                    await Channel.BasicConsumeAsync(queueName, _eventBusSubscriberSettings.AutoAck, consumer);

                    return consumer;
                }
            )
        );
    }

    protected virtual async ValueTask HandleInternalAsync(object? sender, BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;
        
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        var @event = (TEvent)JsonConvert.DeserializeObject(message, typeof(TEvent), _jsonSerializerSettings)!;
        @event.Redelivered = ea.Redelivered;
        
        // Execute with try-catch
        var processEventTask = () => ProcessAsync(@event, cancellationToken);
        var result = await processEventTask.GetValueAsync();

        // If successful, ack / nack based on the result
        if (result.IsSuccess)
        {
            if (result.Data.Result)
                await Channel.BasicAckAsync(ea.DeliveryTag, false);
            else
                await Channel.BasicNackAsync(ea.DeliveryTag, false, result.Data.Requeue);
        }
        else // If failed, nack the message
            await Channel.BasicNackAsync(ea.DeliveryTag, false, false);
        
        
    }

    protected abstract ValueTask<(bool Result, bool Requeue)> ProcessAsync(TEvent @event, CancellationToken cancellationToken);
}