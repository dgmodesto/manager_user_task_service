namespace Sdk.Mediator.Behaviors;

using Abstractions.Enums;
using Abstractions.Interfaces;
using Abstractions.Notifications;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

public class FailFastBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IMediatorHandler _mediator;
    private readonly IEnumerable<IValidator> _validators;

    public FailFastBehavior(IEnumerable<IValidator<TRequest>> validators, IMediatorHandler mediator)
    {
        _validators = validators;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        return failures.Any()
            ? await Errors(failures!)
            : await next();
    }

    private async Task<TResponse> Errors(IEnumerable<ValidationFailure> failures)
    {
        foreach (var failure in failures)
            await _mediator.RaiseEvent(new DomainNotification(FailureReason.BadRequest,
                FailureReason.BadRequest.ToString(), failure.ErrorMessage));

        return default!;
    }
}