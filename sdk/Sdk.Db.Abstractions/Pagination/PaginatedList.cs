namespace Sdk.Db.Abstractions.Pagination;

using System.Text.Json.Serialization;

public class PaginatedList<TEntity> where TEntity : class
{
    public PaginatedList(IQueryable<TEntity>? entities, PaginationObject pagination)
    {
        var results = entities?.ToList();

        Order = pagination.Order!;
        Page = pagination.Page!;
        TotalRecords = pagination.TotalRecords;
        Results = results;
    }

    public IList<TEntity>? Results { get; set; }
    public int TotalRecords { get; set; }

    [JsonIgnore]
    public Page Page { get; set; }

    [JsonIgnore]
    public Order Order { get; set; }

    public int RecordsInPage => _recordsInPage();
    public int CurrentPage => Page.Index;
    public int PageSize => Page.Quantity;
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)Page.Quantity);
    public int FirstRecordOnPage => TotalRecords > 0 ? (CurrentPage - 1) * PageSize + 1 : 0;
    public int LastRecordOnPage => Math.Min(CurrentPage * PageSize, TotalRecords);

    private int _recordsInPage()
    {
        var value = LastRecordOnPage - FirstRecordOnPage + 1;
        return value >= 0 ? value : 0;
    }
}