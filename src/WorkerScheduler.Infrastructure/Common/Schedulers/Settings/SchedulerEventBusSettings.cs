using WorkerScheduler.Application.Common.EventBus.Models;
using WorkerScheduler.Infrastructure.Common.EventBus.Settings;
using WorkerScheduler.Infrastructure.Common.Schedulers.EventSubscribers;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.Settings;

/// <summary>
/// Represents scheduler settings
/// </summary>
public record SchedulerEventBusSettings : EventBusSubscriberSettings<SchedulerEventSubscriber>
{
    /// <summary>
    /// Gets scheduler incoming bus declaration
    /// </summary>
    public BusDeclaration SchedulerIncomingBusDeclaration { get; init; } = default!;
    
    /// <summary>
    /// Gets scheduler outgoing bus declaration
    /// </summary>
    public BusDeclaration SchedulerOutgoingBusDeclaration { get; init; } = default!;
}