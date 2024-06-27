namespace Sdk.Mediator.Abstractions.Responses;

using Enums;

public sealed class Paged<TModel>
    where TModel : class
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int RecordsInPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalRecords { get; set; }

    public string? FilterBy { get; set; }

    public string? OrderBy { get; set; }

    public Sorting Sorting { get; set; }

    public List<TModel>? Records { get; set; }
}