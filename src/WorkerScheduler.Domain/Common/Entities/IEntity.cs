namespace WorkerScheduler.Domain.Common.Entities;

/// <summary>
/// Common entity interface
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Entity Id
    /// </summary>
    public Guid Id { get; set; }
}