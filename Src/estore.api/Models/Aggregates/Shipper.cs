namespace estore.api.Models.Aggregates;

public class Shipper
{
    public int ShipperId { get; }

    public string CompanyName { get; } = string.Empty;

    public string Phone { get; } = string.Empty;
}
