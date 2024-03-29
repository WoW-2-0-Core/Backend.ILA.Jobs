﻿using MediatR;

namespace WorkerScheduler.Domain.Common.Events;

/// <summary>
/// Represents an interface for events in a MediatR-based system.
/// </summary>
public interface IEvent : INotification
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Gets or sets the timestamp when the event was created.
    /// </summary>
    public DateTimeOffset CreatedTime { get; init; }
    
    /// <summary>
    /// Gets or sets a flag indicating whether the event has been redelivered.
    /// </summary>
    public bool Redelivered { get; set; }
}