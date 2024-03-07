using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Application.Common.Schedulers.Events;

/// <summary>
/// Represents event that triggers scheduler to record job history
/// </summary>
public record RecordJobHistory : Event
{
    /// <summary>
    /// Job to be processed
    /// </summary>
    public WorkerJobExecutionHistoryEntity History { get; set; } = default!;
}