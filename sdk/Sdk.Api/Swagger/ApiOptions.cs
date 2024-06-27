namespace Sdk.Api.Swagger;

public class ApiOptions : IApiOptions
{
    public ApiOptions(string? title)
    {
        Title = title ?? "API";
    }

    public string Title { get; set; }
}