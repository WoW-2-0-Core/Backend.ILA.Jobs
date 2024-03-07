using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.EventBus.Models;

namespace WorkerScheduler.Infrastructure.Common.EventBus.Settings;

/// <summary>
/// Represents event bus subscriber settings
/// </summary>
public class EventBusSubscriberSettings<TSubscriber> where TSubscriber : IEventSubscriber
{
    /// <summary>
    /// Gets prefetch message count
    /// </summary>
    public ushort PrefetchCount { get; init; }

    /// <summary>
    /// Gets bus declarations
    /// </summary>
    public ICollection<BusDeclaration> BusDeclarations { get; init; } = default!;
}