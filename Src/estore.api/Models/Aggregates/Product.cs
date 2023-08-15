namespace estore.api.Models.Aggregates;

public class Product
{
    public int ProductId { get; }

    public string ProductName { get; } = string.Empty;

    public int SupplierId { get; }

    public int CategoryId { get; }

    public string QuantityPerUnit { get; } = string.Empty;

    public decimal UnitPrice { get; }

    public int UnitsInStock { get; }

    public int UnitsOnOrder { get; }

    public int ReorderLevel { get; }

    public bool Discontinued { get; }

    public Category Category { get; }

    public Supplier Supplier { get; }
}
