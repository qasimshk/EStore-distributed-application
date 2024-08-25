namespace estore.api.Models.Aggregates;
using estore.api.Extensions;

public sealed class Product
{
    public int ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public int SupplierId { get; private set; }

    public int CategoryId { get; private set; }

    public string QuantityPerUnit { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int UnitsInStock { get; private set; }

    public int UnitsOnOrder { get; private set; }

    public int ReorderLevel { get; private set; }

    public bool Discontinued { get; private set; }

    public Category Category { get; }

    public Supplier Supplier { get; }

    public static Product Create(
        string productName,
        int supplierId,
        int categoryId,
        string quantityPerUnit,
        decimal unitPrice,
        int unitsInStock,
        int unitsOnOrder,
        int reorderLevel,
        bool discontinued) => new()
        {
            ProductId = Guid.NewGuid().GuidToInteger(),
            ProductName = productName,
            SupplierId = supplierId,
            CategoryId = categoryId,
            QuantityPerUnit = quantityPerUnit,
            UnitPrice = unitPrice,
            UnitsInStock = unitsInStock,
            UnitsOnOrder = unitsOnOrder,
            ReorderLevel = reorderLevel,
            Discontinued = discontinued
        };
}
