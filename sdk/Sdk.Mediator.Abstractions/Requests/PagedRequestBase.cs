namespace Sdk.Mediator.Abstractions.Requests;

using Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Responses;

public abstract class PagedRequestBase<TModel> : IRequest<Paged<TModel>?>
    where TModel : class
{
    [FromQuery]
    public int Page { get; set; }

    [FromQuery]
    public int PageSize { get; set; }


    [FromQuery]
    public string? OrderBy { get; set; }

    [FromQuery]
    public Sorting Sorting { get; set; }
}