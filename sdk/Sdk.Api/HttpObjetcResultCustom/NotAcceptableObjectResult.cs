using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sdk.Api.HttpObjetcResultCustom;

/// <summary>
/// An <see cref="ObjectResult"/> that when executed will produce a Conflict (409) response.
/// </summary>
[DefaultStatusCode(DefaultStatusCode)]
public class NotAcceptableObjectResult : ObjectResult
{

    private const int DefaultStatusCode = StatusCodes.Status406NotAcceptable;


    /// <summary>
    /// Creates a new <see cref="NotAcceptableObjectResult"/> instance.
    /// </summary>
    /// <param name="error">Contains the errors to be returned to the client.</param>
    public NotAcceptableObjectResult([ActionResultObjectValue] object? value)
        : base(value)
    {
        StatusCode = DefaultStatusCode;

    }


    /// <summary>
    /// Creates a new <see cref="NotAcceptableObjectResult"/> instance.
    /// </summary>
    /// <param name="modelState"><see cref="ModelStateDictionary"/> containing the validation errors.</param>
    public NotAcceptableObjectResult([ActionResultObjectValue] ModelStateDictionary modelState)
        : base(new SerializableError(modelState))
    {
        StatusCode = DefaultStatusCode;
    }
}
