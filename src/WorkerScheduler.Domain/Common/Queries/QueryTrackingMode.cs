namespace WorkerScheduler.Domain.Common.Queries;

/// <summary>
/// Refers to query tracking mode by ORM
/// </summary>
public enum QueryTrackingMode
{
    /// <summary>
    /// Refers to default tracking mode
    /// </summary>
    AsTracking,
    
    /// <summary>
    /// Refers to no tracking mode
    /// </summary>
    AsNoTracking,
    
    /// <summary>
    /// Refers to no tracking mode with identity resolution
    /// </summary>
    AsNoTrackingWithIdentityResolution
}