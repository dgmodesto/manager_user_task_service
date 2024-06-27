namespace Sdk.Mediator.Abstractions.Requests;

using MediatR;

/// <summary>
///     This class represents a two-way request, that is, a request that expects a response.
/// </summary>
/// <typeparam name="TModel">The expected data model in response</typeparam>
public abstract class TwoWayRequestBase<TModel> : IRequest<TModel?>
    where TModel : class
{
}