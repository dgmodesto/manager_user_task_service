using Sdk.Api.Interfaces;

namespace Sdk.Api.Responses;

public class NotAcceptableResponse<TResponseData> : IApiResponse<TResponseData>
{
    public NotAcceptableResponse(IEnumerable<string>? errors)
    {
        Errors = errors;
    }

    public bool Success => false;

    public TResponseData? Data => default;

    public IEnumerable<string>? Errors { get; }
}

public class NotAcceptableResponse : IApiResponse
{
    public NotAcceptableResponse(IEnumerable<string>? errors)
    {
        Errors = errors;
    }

    public bool Success => false;

    public IEnumerable<string> Errors { get; }
}