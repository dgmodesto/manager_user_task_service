namespace Sdk.Mediator.Abstractions.Handlers;

using Enums;
using Interfaces;
using MediatR;
using Notifications;
using Requests;

public abstract class TwoWayCommandHandler<TCommand, TModel> : IRequestHandler<TCommand, TModel?>
    where TCommand : TwoWayRequestBase<TModel>
    where TModel : class, new()
{
    protected readonly IMediatorHandler Mediator;

    protected TwoWayCommandHandler(IMediatorHandler mediator)
    {
        Mediator = mediator;
    }

    public async Task<TModel?> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await Handle(request);
    }

    protected async Task NotifyError(FailureReason failureReason, string message)
    {
        await Mediator.RaiseEvent(new DomainNotification(failureReason, failureReason.ToString(), message));
    }

    protected abstract Task<TModel?> Handle(TCommand request);
}