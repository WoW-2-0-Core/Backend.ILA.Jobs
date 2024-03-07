namespace WorkerScheduler.Domain.Common.Events;

/// <summary>
/// Represents event options
/// </summary>
public struct EventOptions()
{
    /// <summary>
    /// Event correlation Id in the system
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Gets queue name to reply
    /// </summary>
    public string? ReplyTo { get; set; } 

    /// <summary>
    /// Event exchange name
    /// </summary>
    public string Exchange { get; set; } = default!;

    /// <summary>
    /// Gets event routing key for the exchange
    /// </summary>
    public string RoutingKey { get; set; } = default!;
}