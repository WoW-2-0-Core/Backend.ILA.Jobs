namespace WorkerScheduler.Application.Common.EventBus.Models;

/// <summary>
/// Represents a bus declaration
/// </summary>
public record BusDeclaration
{
    /// <summary>
    /// Gets exchange name
    /// </summary>
    public string ExchangeName { get; init; } = string.Empty;

    /// <summary>
    /// Gets queue name
    /// </summary>
    public string QueueName { get; init; } = string.Empty;

    /// <summary>
    /// Gets binding key
    /// </summary>
    public string BindingKey { get; init; } = string.Empty;

    /// <summary>
    /// Gets routing key
    /// </summary>
    public string RoutingKey { get; init; } = string.Empty;
}