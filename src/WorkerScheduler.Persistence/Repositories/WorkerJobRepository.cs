using System.Linq.Expressions;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Persistence.DataContexts;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Persistence.Repositories;

/// <summary>
/// Provides functionality for worker job repository
/// </summary>
public class WorkerJobRepository(WorkerDbContext dbContext)
    : EntityRepositoryBase<WorkerJobEntity, WorkerDbContext>(dbContext), IWorkerJobRepository
{
    public new IQueryable<WorkerJobEntity> Get(Expression<Func<WorkerJobEntity, bool>>? predicate = default) 
        => base.Get(predicate);

    public new ValueTask<WorkerJobEntity?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(jobId, cancellationToken);

    public new ValueTask<WorkerJobEntity> CreateAsync(WorkerJobEntity job, CancellationToken cancellationToken = default) =>
        base.CreateAsync(job, cancellationToken);

    public new ValueTask<WorkerJobEntity> UpdateAsync(WorkerJobEntity job, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(job, cancellationToken);

    public new ValueTask<bool> DeleteAsync(WorkerJobEntity job, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(job, cancellationToken);

    public new ValueTask<bool> DeleteByIdAsync(Guid jobId, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(jobId, cancellationToken);
}