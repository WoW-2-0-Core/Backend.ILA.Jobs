using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Domain.Enums;

namespace WorkerScheduler.Application.Common.Schedulers.Services;

/// <summary>
/// Defines the contract for a job scheduler.
/// </summary>
public interface IJobSchedulerService
{
    /// <summary>
    /// Gets next scheduled job if exists
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Next job Id along with next execution time</returns>
    ValueTask<(Guid jobId, DateTimeOffset nextJobScheduledTime)?> GetNextScheduledJob(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of jobs in batch
    /// </summary>
    /// <param name="jobs">Jobs to update status</param>
    /// <param name="status">Job status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask UpdateJobsStatusAsync(WorkerJobStatus status, CancellationToken cancellationToken = default, params WorkerJobEntity[] jobs);

    /// <summary>
    /// Publishes jobs to the queue
    /// </summary>
    /// <param name="jobs">Jobs to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask PublishJobsAsync(CancellationToken cancellationToken = default, params WorkerJobEntity[] jobs);

    /// <summary>
    /// Records job history and updates job status
    /// </summary>
    /// <param name="history">History to record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask CreateJobHistoryAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default);
}