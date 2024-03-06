using System.Linq.Expressions;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.Repositories.Interfaces;

/// <summary>
/// Defines functionality for worker job execution history repository
/// </summary>
public interface IWorkerJobExecutionHistoryRepository
{
    /// <summary>
    /// Retrieves worker job execution history records based on optional filtering conditions
    /// </summary>
    /// <param name="predicate">Worker job execution history filter predicate</param>
    /// <returns>Queryable source of worker job execution history records</returns>
    IQueryable<WorkerJobExecutionHistoryEntity> Get(Expression<Func<WorkerJobExecutionHistoryEntity, bool>>? predicate = default);
    
    /// <summary>
    /// Retrieves worker job execution history by its Id
    /// </summary>
    /// <param name="historyId">Worker job execution history Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Worker job execution history if found, otherwise null</returns>
    ValueTask<WorkerJobExecutionHistoryEntity?> GetByIdAsync(Guid historyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new worker job execution history record
    /// </summary>
    /// <param name="history">Worker job execution history record to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created worker job execution history record</returns>
    ValueTask<WorkerJobExecutionHistoryEntity> CreateAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing worker job execution history record
    /// </summary>
    /// <param name="history">Worker job execution history record to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated worker job execution history record</returns>
    ValueTask<WorkerJobExecutionHistoryEntity> UpdateAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing worker job execution history record
    /// </summary>
    /// <param name="history">Worker job execution history record to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    ValueTask<bool> DeleteAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing worker job execution history record by Id
    /// </summary>
    /// <param name="historyId">Id of worker job execution history record to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    ValueTask<bool> DeleteByIdAsync(Guid historyId, CancellationToken cancellationToken = default);
}
