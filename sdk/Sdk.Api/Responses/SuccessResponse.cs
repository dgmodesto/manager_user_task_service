namespace Sdk.Api.Responses;

using Interfaces;

public class SuccessResponse<TResponseData> : IApiResponse<TResponseData>
{
    public SuccessResponse(TResponseData? data)
    {
        Data = data;
    }

    public bool Success => true;

    public TResponseData? Data { get; }

    public IEnumerable<string> Errors => new List<string>();
}

public class SuccessResponse : IApiResponse
{
    public bool Success => true;

    public IEnumerable<string> Errors => new List<string>();
}