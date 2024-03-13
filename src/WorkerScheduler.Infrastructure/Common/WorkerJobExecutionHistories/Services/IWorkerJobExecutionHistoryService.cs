using WorkerScheduler.Application.Common.WorkerJobExecutionHistories.Services;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Infrastructure.Common.WorkerJobExecutionHistories.Services;

/// <summary>
/// Provides worker job execution history service functionality
/// </summary>
public class WorkerJobExecutionHistoryService(IWorkerJobExecutionHistoryRepository workerJobExecutionHistoryRepository) : IWorkerJobExecutionHistoryService
{
    public async ValueTask CreateAsync(WorkerJobExecutionHistoryEntity executionHistory, CancellationToken cancellationToken = default)
    {
        await workerJobExecutionHistoryRepository.CreateAsync(executionHistory, cancellationToken);
    }
}