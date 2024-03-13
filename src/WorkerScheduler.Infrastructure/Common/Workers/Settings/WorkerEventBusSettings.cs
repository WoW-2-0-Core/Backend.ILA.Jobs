using WorkerScheduler.Infrastructure.Common.EventBus.Settings;
using WorkerScheduler.Infrastructure.Common.Workers.Services;

namespace WorkerScheduler.Infrastructure.Common.Workers.Settings;

/// <summary>
/// Represents worker event bus settings
/// </summary>
public record WorkerEventBusSettings : EventBusSubscriberSettings<WorkerEventSubscriber>;
