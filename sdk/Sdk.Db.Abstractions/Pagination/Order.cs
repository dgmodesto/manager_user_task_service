namespace Sdk.Db.Abstractions.Pagination;

public class Order
{
    public Order(string property, bool crescent)
    {
        Property = property;
        Crescent = crescent;
    }

    public Order(string property, string crescent)
        : this(property, Convert.ToBoolean(crescent))
    {
    }

    public string Property { get; set; }
    public bool Crescent { get; set; }
}