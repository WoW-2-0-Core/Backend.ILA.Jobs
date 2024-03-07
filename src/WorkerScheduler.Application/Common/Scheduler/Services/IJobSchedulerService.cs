using System.Collections.Immutable;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Domain.Enums;

namespace WorkerScheduler.Application.Common.Scheduler.Services;

/// <summary>
/// Defines the contract for a job scheduler.
/// </summary>
public interface IJobSchedulerService
{
    ValueTask<(Guid jobId, DateTimeOffset nextJobScheduledTime)?> GetNextScheduledJob(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets failed or scheduled jobs
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of failed or scheduled jobs</returns>
    ValueTask<IImmutableList<WorkerJobEntity>> GetScheduledOrFailedJobsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of jobs in batch
    /// </summary>
    /// <param name="jobs">Jobs to update status</param>
    /// <param name="status">Job status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask UpdateJobsStatus(WorkerJobStatus status, CancellationToken cancellationToken = default, params WorkerJobEntity[] jobs);

    /// <summary>
    /// Publishes jobs to the queue
    /// </summary>
    /// <param name="jobs">Jobs to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask PublishJobsAsync(CancellationToken cancellationToken = default, params WorkerJobEntity[] jobs);
}