namespace Sdk.Mediator.Abstractions.Events;

using MediatR;

public abstract class BaseEvent : IRequest, INotification
{
    protected BaseEvent()
    {
        Timestamp = DateTime.UtcNow;
        MessageType = GetType().Name;
    }

    public string MessageType { get; }

    public DateTime Timestamp { get; }
}