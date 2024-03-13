using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using WorkerScheduler.Domain.Common.Queries;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Persistence.DataContexts;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Persistence.Repositories;

/// <summary>
/// Provides functionality for worker job repository
/// </summary>
public class WorkerJobRepository(WorkerDbContext dbContext) : EntityRepositoryBase<WorkerJobEntity, WorkerDbContext>(dbContext), IWorkerJobRepository
{
    public new IQueryable<WorkerJobEntity> Get(Expression<Func<WorkerJobEntity, bool>>? predicate = default, QueryOptions queryOptions = default) =>
        base.Get(predicate, queryOptions);

    public new ValueTask<WorkerJobEntity?> GetByIdAsync(
        Guid jobId,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default
    ) =>
        base.GetByIdAsync(jobId, queryOptions, cancellationToken);

    public new ValueTask<WorkerJobEntity> CreateAsync(WorkerJobEntity job, CancellationToken cancellationToken = default) =>
        base.CreateAsync(job, cancellationToken);

    public new ValueTask<WorkerJobEntity> UpdateAsync(WorkerJobEntity job, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(job, cancellationToken);

    public new ValueTask<int> UpdateBatchAsync(
        Expression<Func<SetPropertyCalls<WorkerJobEntity>, SetPropertyCalls<WorkerJobEntity>>> setPropertyCalls,
        Expression<Func<WorkerJobEntity, bool>>? batchUpdatePredicate = default,
        CancellationToken cancellationToken = default
    ) =>
        base.UpdateBatchAsync(setPropertyCalls, batchUpdatePredicate, cancellationToken);

    public new ValueTask<bool> DeleteAsync(WorkerJobEntity job, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(job, cancellationToken);

    public new ValueTask<bool> DeleteByIdAsync(Guid jobId, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(jobId, cancellationToken);
}