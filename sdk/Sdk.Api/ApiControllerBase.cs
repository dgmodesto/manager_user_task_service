namespace Sdk.Api;

using Mediator.Abstractions.Enums;
using Mediator.Abstractions.Interfaces;
using Mediator.Abstractions.Notifications;
using Mediator.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Responses;
using Sdk.Api.HttpObjetcResultCustom;
using System.Net;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
public class ApiControllerBase<TModel> : ControllerBase
    where TModel : class
{
    private readonly DomainNotificationHandler _notifications;

    protected ApiControllerBase(IMediatorHandler mediator)
    {
        _notifications = (DomainNotificationHandler)mediator.GetNotificationHandler();
        _mediator = mediator;
    }

    protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

    protected IMediatorHandler _mediator { get; }

    protected bool IsValidOperation()
    {
        return !_notifications.HasNotifications();
    }

    protected async Task NotifyError(FailureReason failureReason, string message)
    {
        await _mediator.RaiseEvent(new DomainNotification(failureReason, failureReason.ToString(), message));
    }

    protected ActionResult ApiResponse(HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        if (IsValidOperation())
            return httpStatusCode switch
            {
                HttpStatusCode.OK => Ok(new SuccessResponse()),
                HttpStatusCode.Created => Created("", new SuccessResponse()),
                HttpStatusCode.Accepted => Accepted(new SuccessResponse()),
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.BadRequest => BadRequest(_notifications.GetNotifications().Select(n => n.Value)),
                HttpStatusCode.NotAcceptable => NotAcceptable(new NotAcceptableResponse(_notifications.GetNotifications().Select(n => n.Value))),
                _ => Problem(null, null, (int)httpStatusCode)
            };

        return HandleFailure();
    }

    protected ActionResult ApiResponse<TValue>(TValue? response, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        if (IsValidOperation())
            return httpStatusCode switch
            {
                HttpStatusCode.OK => Ok(new SuccessResponse<TValue>(response)),
                HttpStatusCode.Created => Created("", new SuccessResponse<TValue>(response)),
                HttpStatusCode.Accepted => Accepted(new SuccessResponse<TValue>(response)),
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.BadRequest => BadRequest(_notifications.GetNotifications().Select(n => n.Value)),
                HttpStatusCode.NotAcceptable => NotAcceptable(new NotAcceptableResponse<TValue>(_notifications.GetNotifications().Select(n => n.Value))),
                _ => Problem(null, null, (int)httpStatusCode)
            };

        return HandleFailure();
    }

    private ActionResult HandleFailure()
    {
        if (HasFailure(FailureReason.BadRequest))
            return BadRequest(new BadRequestResponse<TModel>(_notifications.GetNotifications().Select(n => n.Value)));

        if (HasFailure(FailureReason.NotFound))
            return NotFound(new NotFoundResponse<TModel>(_notifications.GetNotifications().Select(n => n.Value)));

        if (HasFailure(FailureReason.Conflict))
            return Conflict(new ConflictResponse<TModel>(_notifications.GetNotifications().Select(n => n.Value)));

        if (HasFailure(FailureReason.NotAcceptable))
            return NotAcceptable(new NotAcceptableResponse<TModel>(_notifications.GetNotifications().Select(n => n.Value)));

        return InternalError(_notifications.GetNotifications().Select(n => n.Value));
    }

    private ObjectResult InternalError([ActionResultObjectValue] IEnumerable<string> errors)
    {
        return Problem(string.Join(", ", errors), null, (int)HttpStatusCode.InternalServerError);
    }

    private bool HasFailure(FailureReason failureReason)
    {
        return _notifications.GetNotifications().Any(n => n.FailureReason == failureReason);
    }

    protected NotAcceptableObjectResult NotAcceptable([ActionResultObjectValue] object value)
        => new NotAcceptableObjectResult(value);
}