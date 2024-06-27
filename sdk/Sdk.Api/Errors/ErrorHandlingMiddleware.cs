using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sdk.Api.Exceptions;
using System.Net;

namespace Sdk.Api.Errors;

public class ErrorHandlingMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private Task HandleException(HttpContext context, Exception ex)
    {
        switch (ex)
        {
            case BadRequestException:
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return context.Response.WriteAsync(ex.Message);
            case NotFoundException:
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                return context.Response.WriteAsync(ex.Message);
            case ConflitException:
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;

                return context.Response.WriteAsync(ex.Message);
            case UnauthorizedException:
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return context.Response.WriteAsync(ex.Message);
            case ForbiddenException:
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                return context.Response.WriteAsync(ex.Message);
            default:
                var errorTag = Guid.NewGuid();
                _logger.LogError(ex, "An error occurred, see logs. Error tag: {ErrorTag}", errorTag);

                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return context.Response.WriteAsync($"An error occurred, see logs. Error tag: {errorTag}");
        }
    }
}