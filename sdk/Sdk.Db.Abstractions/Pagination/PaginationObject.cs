namespace Sdk.Db.Abstractions.Pagination;

public class PaginationObject
{
    public Page? Page { get; set; }
    public Order? Order { get; set; }
    public int TotalRecords { get; set; }
}