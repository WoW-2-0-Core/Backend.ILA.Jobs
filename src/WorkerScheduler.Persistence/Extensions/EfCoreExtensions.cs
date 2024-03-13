using Microsoft.EntityFrameworkCore;
using WorkerScheduler.Domain.Common.Queries;

namespace WorkerScheduler.Persistence.Extensions;

/// <summary>
/// Contains extensions for Entity Framework Core
/// </summary>
public static class EfCoreExtensions
{
    /// <summary>
    /// Applies tracking mode to the queryable source
    /// </summary>
    /// <param name="source">Queryable source to apply tracking mode</param>
    /// <param name="trackingMode">Tracking mode to apply</param>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <returns>Queryable source with tracking mode applied</returns>
    public static IQueryable<TSource> ApplyTrackingMode<TSource>(this IQueryable<TSource> source, QueryTrackingMode trackingMode) where TSource : class
    {
        return trackingMode switch
        {
            QueryTrackingMode.AsTracking => source,
            QueryTrackingMode.AsNoTracking => source.AsNoTracking(),
            QueryTrackingMode.AsNoTrackingWithIdentityResolution => source.AsNoTrackingWithIdentityResolution(),
            _ => source
        };
    }
}