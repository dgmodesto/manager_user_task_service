namespace Sdk.Mediator.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var serialized = JsonSerializer.Serialize(request);
        _logger.LogInformation(serialized);

        var result = next().Result;

        return Task.FromResult(result);
    }
}