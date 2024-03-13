using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Application.Common.WorkerJobExecutionHistories.Services;

/// <summary>
/// Defines worker job execution history service functionality
/// </summary>
public interface IWorkerJobExecutionHistoryService
{
    /// <summary>
    /// Creates worker job execution history
    /// </summary>
    /// <param name="executionHistory">History to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask CreateAsync(WorkerJobExecutionHistoryEntity executionHistory, CancellationToken cancellationToken = default);
}