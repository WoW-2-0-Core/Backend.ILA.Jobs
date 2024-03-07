namespace WorkerScheduler.Infrastructure.Common.EventBus.Settings;

/// <summary>
/// Represents RabbitMQ connection settings
/// </summary>
public record RabbitMqConnectionSettings
{
    /// <summary>
    /// Gets hostname
    /// </summary>
    public string HostName { get; init; } = default!;
    
    /// <summary>
    /// Gets port
    /// </summary>
    public int Port { get; init; }
}