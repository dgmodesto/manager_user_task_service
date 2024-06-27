namespace Sdk.Mediator.Notifications;

using Abstractions.Notifications;
using MediatR;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>, IDisposable
{
    private bool _disposed;
    private List<DomainNotification> _notifications;

    public DomainNotificationHandler()
    {
        _notifications = new List<DomainNotification>();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    public Task Handle(DomainNotification message, CancellationToken cancellationToken)
    {
        GetNotifications().Add(message);

        return Task.CompletedTask;
    }

    public virtual List<DomainNotification> GetNotifications()
    {
        return _notifications;
    }

    public virtual bool HasNotifications()
    {
        return GetNotifications().Any();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing) _notifications = new List<DomainNotification>();

        _disposed = true;
    }
}