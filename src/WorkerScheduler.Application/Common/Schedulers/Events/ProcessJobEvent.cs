using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Application.Common.Schedulers.Events;

/// <summary>
/// Represents event that triggers workers to process event
/// </summary>
public record ProcessJobEvent : Event
{
    /// <summary>
    /// Gets job to be processed
    /// </summary>
    public WorkerJobEntity Job { get; init; } = default!;

    /// <summary>
    /// Gets execution history Id if this retry event
    /// </summary>
    public Guid ParentHistoryId { get; init; } = default!;
}