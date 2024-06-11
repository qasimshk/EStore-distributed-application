namespace estore.common.Events;

using System;

public abstract class BaseEvent
{
    public Guid CorrelationId { get; init; }
}
