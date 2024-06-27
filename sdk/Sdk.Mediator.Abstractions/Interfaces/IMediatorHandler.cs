namespace Sdk.Mediator.Abstractions.Interfaces;

using Events;
using MediatR;
using Notifications;
using Requests;
using Responses;

public interface IMediatorHandler
{
    Task SendCommand(IRequest request);

    Task<TModel?> SendCommand<TModel>(IRequest<TModel?> request)
        where TModel : class;

    Task<Paged<TModel>?> SendPagedQuery<TModel>(PagedRequestBase<TModel> request)
        where TModel : class;

    Task<TModel?> SendQuery<TModel>(IRequest<TModel?> request)
        where TModel : class;

    Task RaiseEvent<T>(T @event)
        where T : BaseEvent;

    bool HasNotification();

    INotificationHandler<DomainNotification> GetNotificationHandler();
}