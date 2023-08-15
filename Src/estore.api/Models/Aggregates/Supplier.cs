namespace estore.api.Models.Aggregates;

public class Supplier
{
    public int SupplierId { get; }

    public string CompanyName { get; } = string.Empty;

    public string ContactName { get; } = string.Empty;

    public string ContactTitle { get; } = string.Empty;

    public string Phone { get; } = string.Empty;

    public string Fax { get; } = string.Empty;

    public string HomePage { get; } = string.Empty;

    public Addresses SupplierAddress { get; }

    public Product? Product { get; set; }
}
