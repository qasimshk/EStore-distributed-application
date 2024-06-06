namespace estore.api.Models.Responses;

public class CustomerResponse
{
    public string CustomerId { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContactTitle { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public List<CustomerOrderResponse> CustomerOrders { get; set; } = [];
}
