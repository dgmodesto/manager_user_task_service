namespace Sdk.Api.Responses;

using Interfaces;

public class ConflictResponse<TResponseData> : IApiResponse<TResponseData>
{
    public ConflictResponse(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public bool Success => false;

    public TResponseData? Data => default;

    public IEnumerable<string> Errors { get; }
}