using System.Linq.Expressions;
using WorkerScheduler.Application.Common.WorkerJobs.Services;
using WorkerScheduler.Domain.Common.Queries;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Infrastructure.Common.WorkerJobs.Services;

/// <summary>
/// Defines interface for worker job service
/// </summary>
public class WorkerJobService(IWorkerJobRepository workerJobRepository) : IWorkerJobService
{
    public IQueryable<WorkerJobEntity> Get(
        Expression<Func<WorkerJobEntity, bool>>? predicate = default, 
        QueryOptions queryOptions = default) =>
        workerJobRepository.Get(predicate, queryOptions);

    public ValueTask<WorkerJobEntity?> GetByIdAsync(
        Guid workerJobId,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default
    )
    => workerJobRepository.GetByIdAsync(workerJobId, queryOptions, cancellationToken);
}