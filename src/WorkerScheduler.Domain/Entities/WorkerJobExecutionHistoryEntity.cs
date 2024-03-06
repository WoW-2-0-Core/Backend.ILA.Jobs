using WorkerScheduler.Domain.Common.Entities;

namespace WorkerScheduler.Domain.Entities;

/// <summary>
/// Represents worker specific job execution history
/// </summary>
public class WorkerJobExecutionHistoryEntity : IEntity
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets related job Id
    /// </summary>
    public Guid JobId { get; set; }
    
    /// <summary>
    /// Gets or sets success result indicator
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Get or sets history related worker job 
    /// </summary>
    public WorkerJobEntity Job { get; set; } = default!;
}