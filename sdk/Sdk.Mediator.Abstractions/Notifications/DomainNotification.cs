namespace Sdk.Mediator.Abstractions.Notifications;

using Enums;
using Events;

public class DomainNotification : BaseEvent
{
    public DomainNotification(FailureReason failureReason, string key, string value)
    {
        NotificationId = Guid.NewGuid();
        FailureReason = failureReason;
        Version = 1;
        Key = key;
        Value = value;
    }

    public Guid NotificationId { get; }

    public FailureReason FailureReason { get; }

    public string Key { get; }

    public string Value { get; }

    public int Version { get; }
}