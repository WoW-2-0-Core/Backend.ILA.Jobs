using System.Linq.Expressions;
using WorkerScheduler.Domain.Common.Queries;
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
    public new IQueryable<WorkerJobExecutionHistoryEntity> Get(
        Expression<Func<WorkerJobExecutionHistoryEntity, bool>>? predicate = default,
        QueryOptions queryOptions = default
    ) =>
        base.Get(predicate, queryOptions);

    public new ValueTask<WorkerJobExecutionHistoryEntity?> GetByIdAsync(
        Guid historyId,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default
    ) =>
        base.GetByIdAsync(historyId, queryOptions, cancellationToken);

    public new ValueTask<WorkerJobExecutionHistoryEntity> CreateAsync(
        WorkerJobExecutionHistoryEntity history,
        CancellationToken cancellationToken = default
    ) =>
        base.CreateAsync(history, cancellationToken);

    public new ValueTask<WorkerJobExecutionHistoryEntity> UpdateAsync(
        WorkerJobExecutionHistoryEntity history,
        CancellationToken cancellationToken = default
    ) =>
        base.UpdateAsync(history, cancellationToken);

    public new ValueTask<bool> DeleteAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(history, cancellationToken);

    public new ValueTask<bool> DeleteByIdAsync(Guid historyId, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(historyId, cancellationToken);
}