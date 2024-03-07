using System.Text;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Domain.Common.Events;

namespace WorkerScheduler.Infrastructure.Common.EventBus.Brokers;

public class RabbitMqEventBusBroker(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider,
    IPublisher mediator
    ) : IEventBusBroker
{
    public ValueTask PublishLocalAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        return new ValueTask(mediator.Publish(@event, cancellationToken));
    }

    public async ValueTask PublishAsync<TEvent>(TEvent @event, EventOptions eventOptions, CancellationToken cancellationToken) where TEvent : Event
    {
        var channel = await rabbitMqConnectionProvider.CreateChannelAsync();
        
        var properties = new BasicProperties
        {
            Persistent = true,
        };
        
        if(eventOptions.CorrelationId is not null)
            properties.CorrelationId = eventOptions.CorrelationId;
        
        if(eventOptions.ReplyTo is not null)
            properties.ReplyTo = eventOptions.ReplyTo;

        var serializerSettings = jsonSerializationSettingsProvider.Get(true);
        serializerSettings.ContractResolver = new DefaultContractResolver();
        serializerSettings.TypeNameHandling = TypeNameHandling.All;

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, serializerSettings));
        await channel.BasicPublishAsync(eventOptions.Exchange, eventOptions.RoutingKey, properties, body);
    }
}