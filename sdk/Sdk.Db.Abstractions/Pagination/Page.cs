namespace Sdk.Db.Abstractions.Pagination;

public class Page
{
    public Page(int index, int quantity)
    {
        Index = index < 1 ? 1 : index;
        Quantity = quantity < 1 ? 1 : quantity;
    }

    public int Index { get; set; }
    public int Quantity { get; set; }
}