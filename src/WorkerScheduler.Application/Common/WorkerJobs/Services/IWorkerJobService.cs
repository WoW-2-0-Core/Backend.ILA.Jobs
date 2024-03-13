using System.Linq.Expressions;
using WorkerScheduler.Domain.Common.Queries;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Application.Common.WorkerJobs.Services;

/// <summary>
/// Defines interface for worker job service
/// </summary>
public interface IWorkerJobService
{
    /// <summary>
    /// Gets queryable source of worker jobs based on predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter the <see cref="WorkerJobEntity"/> entities.</param>
    /// <param name="queryOptions">Querying options</param>
    /// <returns>Queryable source of <see cref="WorkerJobEntity"/></returns>
    IQueryable<WorkerJobEntity> Get(Expression<Func<WorkerJobEntity, bool>>? predicate = default, QueryOptions queryOptions = default);

    /// <summary>
    /// Gets worker job by Id
    /// </summary>
    /// <param name="workerJobId">Id of worker job to query</param>
    /// <param name="queryOptions">Querying options</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Worker job if found, otherwise null</returns>
    ValueTask<WorkerJobEntity?> GetByIdAsync(Guid workerJobId, QueryOptions queryOptions = default, CancellationToken cancellationToken = default);
}