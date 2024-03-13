using WorkerScheduler.Domain.Common.Events;

namespace WorkerScheduler.Application.Common.Schedulers.Events;

/// <summary>
/// Represents event that triggers scheduler to record job history
/// </summary>
public record RecordJobHistoryEvent : Event
{
    /// <summary>
    /// Gets worker job Id
    /// </summary>
    public Guid JobId { get; init; }
    
    /// <summary>
    /// Gets execution history Id if this retry event
    /// </summary>
    public Guid? ParentHistoryId { get; init; } = default!;
    
    /// <summary>
    /// Gets or sets retry count
    /// </summary>
    public byte RetryCount { get; set; }
    
    /// <summary>
    /// Gets success result indicator
    /// </summary>
    public bool IsSuccessful { get; init; } = default!;
}