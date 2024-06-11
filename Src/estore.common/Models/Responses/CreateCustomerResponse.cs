namespace estore.common.Models.Responses;

public class CreateCustomerResponse
{
    public string CustomerID { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContactTitle { get; set; } = string.Empty;

    public string? CustomerAddress { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Fax { get; set; } = string.Empty;
}
