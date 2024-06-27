namespace Sdk.Mediator;

using Abstractions.Events;
using Abstractions.Interfaces;
using Abstractions.Notifications;
using Abstractions.Requests;
using Abstractions.Responses;
using MediatR;
using Notifications;

public sealed class InMemoryBus : IMediatorHandler
{
    private readonly IMediator _mediator;
    private readonly DomainNotificationHandler _notificationHandler;

    public InMemoryBus(
        IMediator mediator,
        INotificationHandler<DomainNotification> domainNotification)
    {
        _mediator = mediator ?? throw new ArgumentException(null, nameof(mediator));
        _notificationHandler = (DomainNotificationHandler)domainNotification ??
                               throw new ArgumentException(null, nameof(domainNotification));
    }

    public Task RaiseEvent<T>(T @event) where T : BaseEvent
    {
        return _mediator.Publish(@event);
    }

    public INotificationHandler<DomainNotification> GetNotificationHandler()
    {
        return _notificationHandler;
    }

    public bool HasNotification()
    {
        return _notificationHandler.HasNotifications();
    }

    public Task<TModel?> SendQuery<TModel>(IRequest<TModel?> request) where TModel : class
    {
        return _mediator.Send(request);
    }

    public Task<Paged<TModel>?> SendPagedQuery<TModel>(PagedRequestBase<TModel> request) where TModel : class
    {
        return _mediator.Send(request);
    }

    public Task<TModel?> SendCommand<TModel>(IRequest<TModel?> request) where TModel : class
    {
        return _mediator.Send(request);
    }

    public Task SendCommand(IRequest request)
    {
        return _mediator.Send(request);
    }
}