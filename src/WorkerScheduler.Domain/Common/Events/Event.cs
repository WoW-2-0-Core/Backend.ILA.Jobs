namespace WorkerScheduler.Domain.Common.Events;

/// <summary>
/// Represents an implementation of the IEvent interface, defining the properties for an event.
/// </summary>
public record Event : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public DateTimeOffset CreatedTime { get; init; } = DateTimeOffset.UtcNow;

    public bool Redelivered { get; set; }
}