namespace WorkerScheduler.Application.Common.EventBus.EventSubscribers;

/// <summary>
/// Defines event bus broker functionality
/// </summary>
public interface IEventSubscriber
{
    /// <summary>
    /// Starts the event subscriber asynchronously.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask StartAsync(CancellationToken token);

    /// <summary>
    /// Stops the event subscriber asynchronously.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask StopAsync(CancellationToken token);
}