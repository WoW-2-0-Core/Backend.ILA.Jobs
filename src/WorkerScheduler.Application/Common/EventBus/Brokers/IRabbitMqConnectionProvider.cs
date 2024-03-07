using RabbitMQ.Client;

namespace WorkerScheduler.Application.Common.EventBus.Brokers;

/// <summary>
/// Defines rabbit mq connection provides functionality
/// </summary>
public interface IRabbitMqConnectionProvider
{
    /// <summary>
    /// Creates a RabbitMQ channel for communication.
    /// </summary>
    /// <returns>Created channel</returns>
    ValueTask<IChannel> CreateChannelAsync();
}