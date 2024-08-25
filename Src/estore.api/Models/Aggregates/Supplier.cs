namespace estore.api.Models.Aggregates;
using estore.api.Extensions;

public sealed class Supplier
{
    public int SupplierId { get; private set; }

    public string CompanyName { get; private set; } = string.Empty;

    public string ContactName { get; private set; } = string.Empty;

    public string ContactTitle { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public string Fax { get; private set; } = string.Empty;

    public string HomePage { get; private set; } = string.Empty;

    public Addresses SupplierAddress { get; private set; }

    public Product? Product { get; private set; }

    public static Supplier Create(string companyName,
        string contactName,
        string contactTitle,
        string phone,
        string fax,
        string homePage,
        Addresses addresses) => new()
        {
            SupplierId = Guid.NewGuid().GuidToInteger(),
            CompanyName = companyName,
            ContactName = contactName,
            ContactTitle = contactTitle,
            Phone = phone,
            Fax = fax,
            HomePage = homePage,
            SupplierAddress = addresses
        };
}
