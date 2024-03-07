using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Application.Common.Scheduler.Events;

/// <summary>
/// Represents event that triggers workers to process event
/// </summary>
public class ProcessJobEvent : Event
{
    /// <summary>
    /// Job to be processed
    /// </summary>
    public WorkerJobEntity Job { get; set; } = default!;
}