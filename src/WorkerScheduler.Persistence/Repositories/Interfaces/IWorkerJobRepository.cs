using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.Repositories.Interfaces;

/// <summary>
/// Defines functionality for worker job repository
/// </summary>
public interface IWorkerJobRepository
{
    /// <summary>
    /// Retrieves worker jobs based on optional filtering conditions
    /// </summary>
    /// <param name="predicate">Worker job filter predicate</param>
    /// <returns>Queryable source of worker jobs</returns>
    IQueryable<WorkerJobEntity> Get(Expression<Func<WorkerJobEntity, bool>>? predicate = default);

    /// <summary>
    /// Retrieves worker job by its Id
    /// </summary>
    /// <param name="jobId">Worker job Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Worker job if found, otherwise null</returns>
    ValueTask<WorkerJobEntity?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new worker job
    /// </summary>
    /// <param name="job">Worker job to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created worker job</returns>
    ValueTask<WorkerJobEntity> CreateAsync(WorkerJobEntity job, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing worker job
    /// </summary>
    /// <param name="job">Worker job to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated worker job</returns>
    ValueTask<WorkerJobEntity> UpdateAsync(WorkerJobEntity job, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates worker jobs in batch
    /// </summary>
    /// <param name="batchUpdatePredicate">Predicate to select worker jobs for batch update</param>
    /// <param name="setPropertyCalls">Batch update value selectors</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of updated rows.</returns>
    ValueTask<int> UpdateBatchAsync(
        Expression<Func<SetPropertyCalls<WorkerJobEntity>, SetPropertyCalls<WorkerJobEntity>>> setPropertyCalls,
        Expression<Func<WorkerJobEntity, bool>>? batchUpdatePredicate = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Deletes an existing worker job
    /// </summary>
    /// <param name="job">Worker job to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    ValueTask<bool> DeleteAsync(WorkerJobEntity job, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing worker job by Id
    /// </summary>
    /// <param name="jobId">Id of worker job to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    ValueTask<bool> DeleteByIdAsync(Guid jobId, CancellationToken cancellationToken = default);
}