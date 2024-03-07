﻿using WorkerScheduler.Application.Common.EventBus.Brokers;

namespace WorkerScheduler.Infrastructure.Common.EventBus.Settings;

/// <summary>
/// Represents event bus subscriber settings
/// </summary>
public class EventBusSubscriberSettings<TSubscriber> where TSubscriber : IEventSubscriber
{
    /// <summary>
    /// Gets prefetch message count
    /// </summary>
    public ushort PrefetchCount { get; init; }
}