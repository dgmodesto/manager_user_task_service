namespace Sdk.Mediator.Abstractions.Handlers;

using Enums;
using Interfaces;
using MediatR;
using Notifications;

public abstract class QueryHandler<TQuery, TData> : IRequestHandler<TQuery, TData?>
    where TQuery : IRequest<TData?>
    where TData : class
{
    protected readonly IMediatorHandler Mediator;

    protected QueryHandler(IMediatorHandler mediator)
    {
        Mediator = mediator;
    }

    public async Task<TData?> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await Handle(request);
    }

    protected async Task NotifyError(FailureReason failureReason, string message)
    {
        await Mediator.RaiseEvent(new DomainNotification(failureReason, failureReason.ToString(), message));
    }

    protected abstract Task<TData?> Handle(TQuery request);
}