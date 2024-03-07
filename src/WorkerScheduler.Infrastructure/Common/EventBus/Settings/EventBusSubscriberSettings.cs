using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.EventBus.EventSubscribers;

namespace WorkerScheduler.Infrastructure.Common.EventBus.Settings;

/// <summary>
/// Represents event bus subscriber settings
/// </summary>
public abstract record EventBusSubscriberSettings<TSubscriber> where TSubscriber : IEventSubscriber
{
    /// <summary>
    /// Gets prefetch message count
    /// </summary>
    public ushort PrefetchCount { get; init; }
}