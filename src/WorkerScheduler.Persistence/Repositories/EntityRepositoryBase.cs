using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WorkerScheduler.Domain.Common.Entities;

namespace WorkerScheduler.Persistence.Repositories;

/// <summary>
/// Provides CRUD functionality for entity repositories
/// </summary>
public abstract class EntityRepositoryBase<TEntity, TContext>(TContext dbContext) where TEntity : class, IEntity where TContext : DbContext
{
    protected TContext DbContext => dbContext;

    /// <summary>
    /// Retrieves entities from the repository based on optional filtering conditions
    /// </summary>
    /// <param name="predicate">Entity filter predicate</param>
    /// <returns>Queryable source entities</returns>
    protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = default)
    {
        var initialQuery = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
            initialQuery = initialQuery.Where(predicate);

        return initialQuery;
    }
    
    /// <summary>
    /// Retrieves entities by its Id
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity if found, otherwise null</returns>
    protected async ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var initialQuery = DbContext.Set<TEntity>().AsQueryable();

        return await initialQuery.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="entity">Entity to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created entity</returns>
    protected async ValueTask<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated entity</returns>
    protected async ValueTask<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
    
    /// <summary>
    /// Updates entities in batch
    /// </summary>
    /// <param name="batchUpdatePredicate">Predicate to select entities for batch update</param>
    /// <param name="setPropertyCalls">Batch update value selectors</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of updated rows.</returns>
    protected async ValueTask<int> UpdateBatchAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        // IImmutableList<(Func<TEntity, object> propertySelector, Func<TEntity, object> valueSelector)> setPropertyExecutors,
        Expression<Func<TEntity, bool>>? batchUpdatePredicate = default,
        CancellationToken cancellationToken = default
    )
    {
        var entities = DbContext.Set<TEntity>().AsQueryable();

        if (batchUpdatePredicate is not null)
            entities = entities.Where(batchUpdatePredicate);

        return await entities.ExecuteUpdateAsync(setPropertyCalls,
            cancellationToken
        );
    }

    /// <summary>
    /// Deletes an existing entity
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    protected async ValueTask<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Remove(entity);
        return await DbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// Deletes an existing entity by Id
    /// </summary>
    /// <param name="entityId">Id of entity to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    protected async ValueTask<bool> DeleteByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
    {
        var foundEntity = await DbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == entityId, cancellationToken) ??
                          throw new InvalidOperationException("Couldn't delete entity, entity doesn't exist");

        return await DeleteAsync(foundEntity, cancellationToken);
    }
}