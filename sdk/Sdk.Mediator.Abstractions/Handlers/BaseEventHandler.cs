namespace Sdk.Mediator.Abstractions.Handlers;

using Enums;
using Interfaces;
using MediatR;
using Notifications;

public abstract class BaseEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification
{
    protected readonly IMediatorHandler Mediator;

    protected BaseEventHandler(IMediatorHandler mediator)
    {
        Mediator = mediator;
    }

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        await Handle(notification);
    }

    protected async Task NotifyError(FailureReason failureReason, string message)
    {
        await Mediator.RaiseEvent(new DomainNotification(failureReason, failureReason.ToString(), message));
    }

    protected abstract Task Handle(TEvent @event);
}