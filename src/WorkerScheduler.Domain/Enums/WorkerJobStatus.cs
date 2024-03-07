namespace WorkerScheduler.Domain.Enums;

/// <summary>
/// Represents worker job status in the system
/// </summary>
public enum WorkerJobStatus
{
    /// <summary>
    /// Refers to a job that is scheduled to run
    /// </summary>
    Scheduled,
    
    /// <summary>
    /// Refers to a job that is queued to runs
    /// </summary>
    Queued,
    
    /// <summary>
    /// Refers to a job that is running
    /// </summary>
    Running, 
    
    /// <summary>
    /// Refers to a job that is completed
    /// </summary>
    Completed,
    
    /// <summary>
    /// Refers to a job that has failed
    /// </summary>
    Failed
}