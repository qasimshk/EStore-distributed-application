namespace estore.common.Models.Responses;

public class ProductResponse
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public int SupplierId { get; set; }

    public int CategoryId { get; set; }

    public string QuantityPerUnit { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public int UnitsOnOrder { get; set; }

    public int ReorderLevel { get; set; }

    public bool Discontinued { get; set; }
}
