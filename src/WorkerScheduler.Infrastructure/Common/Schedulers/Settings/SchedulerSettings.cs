using WorkerScheduler.Application.Common.EventBus.Models;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.Settings;

/// <summary>
/// Represents scheduler settings
/// </summary>
public record SchedulerSettings
{
    /// <summary>
    /// Gets bus declarations for the scheduler
    /// </summary>
    public BusDeclaration BusDeclaration { get; init; } = default!;
}