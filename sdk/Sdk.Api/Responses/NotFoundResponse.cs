namespace Sdk.Api.Responses;

using Interfaces;

public class NotFoundResponse<TResponseData> : IApiResponse<TResponseData>
{
    public NotFoundResponse(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public bool Success => false;

    public TResponseData? Data => default;

    public IEnumerable<string> Errors { get; }
}