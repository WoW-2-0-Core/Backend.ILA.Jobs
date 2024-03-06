using System.Linq.Expressions;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Persistence.DataContexts;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Persistence.Repositories;

/// <summary>
/// Provides functionality for worker job execution history repository
/// </summary>
public class WorkerJobExecutionHistoryRepository(WorkerDbContext dbContext)
    : EntityRepositoryBase<WorkerJobExecutionHistoryEntity, WorkerDbContext>(dbContext), IWorkerJobExecutionHistoryRepository
{
    public new IQueryable<WorkerJobExecutionHistoryEntity> Get(Expression<Func<WorkerJobExecutionHistoryEntity, bool>>? predicate = default) =>
        base.Get(predicate);

    public new ValueTask<WorkerJobExecutionHistoryEntity?> GetByIdAsync(Guid historyId, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(historyId, cancellationToken);

    public new ValueTask<WorkerJobExecutionHistoryEntity> CreateAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default) =>
        base.CreateAsync(history, cancellationToken);

    public new ValueTask<WorkerJobExecutionHistoryEntity> UpdateAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(history, cancellationToken);

    public new ValueTask<bool> DeleteAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(history, cancellationToken);

    public new ValueTask<bool> DeleteByIdAsync(Guid historyId, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(historyId, cancellationToken);
}