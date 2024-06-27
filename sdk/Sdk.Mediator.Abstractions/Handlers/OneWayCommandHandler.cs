namespace Sdk.Mediator.Abstractions.Handlers;

using Enums;
using Interfaces;
using MediatR;
using Notifications;
using Requests;

public abstract class OneWayCommandHandler<TCommand> : AsyncRequestHandler<TCommand>
    where TCommand : OneWayRequestBase
{
    protected readonly IMediatorHandler Mediator;

    protected OneWayCommandHandler(IMediatorHandler mediator)
    {
        Mediator = mediator;
    }

    protected override async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        await Handle(request);
    }

    protected async Task NotifyError(FailureReason failureReason, string message)
    {
        await Mediator.RaiseEvent(new DomainNotification(failureReason, failureReason.ToString(), message));
    }

    protected abstract Task Handle(TCommand request);
}