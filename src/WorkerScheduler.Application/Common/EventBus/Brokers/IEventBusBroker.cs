using WorkerScheduler.Domain.Common.Events;

namespace WorkerScheduler.Application.Common.EventBus.Brokers;

/// <summary>
/// Represents a generic interface for an event bus broker, which facilitates the publishing and subscribing to events.
/// </summary>
public interface IEventBusBroker
{
    /// <summary>
    /// Publishes event local in process event bus
    /// </summary>
    /// <param name="event">Event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TEvent">Type of event to publish</typeparam>
    ValueTask PublishLocalAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;

    /// <summary>
    /// Publishes event to out of process event bus
    /// </summary>
    /// <param name="event">Event to publish</param>
    /// <param name="eventOptions">Event publishing / execution options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TEvent"></typeparam>
    ValueTask PublishAsync<TEvent>(TEvent @event, EventOptions eventOptions, CancellationToken cancellationToken) where TEvent : Event;
}