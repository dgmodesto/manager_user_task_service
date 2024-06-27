namespace Sdk.Api.Responses;

using Interfaces;

public class BadRequestResponse<TResponseData> : IApiResponse<TResponseData>
{
    public BadRequestResponse(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public bool Success => false;

    public TResponseData? Data => default;

    public IEnumerable<string> Errors { get; }
}