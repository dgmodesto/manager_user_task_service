namespace Sdk.Mediator.Abstractions.Requests;

using MediatR;

/// <summary>
///     This class represents a one-way request, that is, a request that does not expect a response (fire and forget).
/// </summary>
public abstract class OneWayRequestBase : IRequest
{
}