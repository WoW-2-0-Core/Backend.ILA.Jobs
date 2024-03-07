using NCrontab;
using WorkerScheduler.Domain.Common.Entities;
using WorkerScheduler.Domain.Enums;

namespace WorkerScheduler.Domain.Entities;

/// <summary>
/// Represents scheduled worker job
/// </summary>
public class WorkerJobEntity : IEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets job type that determines the worker to be executed
    /// </summary>
    public string JobType { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets current job status
    /// </summary>
    public WorkerJobStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets job schedule in cron format
    /// </summary>
    public CrontabSchedule CronSchedule { get; set; } = default!;
}