namespace Sdk.Api.Interfaces;

internal interface IApiResponse<TResponseData>
{
    bool Success { get; }
    public TResponseData? Data { get; }
    public IEnumerable<string> Errors { get; }
}

internal interface IApiResponse
{
    bool Success { get; }
    public IEnumerable<string> Errors { get; }
}