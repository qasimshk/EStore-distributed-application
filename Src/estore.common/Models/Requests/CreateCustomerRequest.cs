namespace estore.common.Models.Requests;

using estore.common.Events;

public class CreateCustomerRequest
{
    public string CompanyName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContactTitle { get; set; } = string.Empty;

    public string? Address { get; set; } = string.Empty;

    public string? City { get; set; } = string.Empty;

    public string? Region { get; set; } = string.Empty;

    public string? PostalCode { get; set; } = string.Empty;

    public string? Country { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Fax { get; set; } = string.Empty;

    public static CreateCustomerRequest Map(CreateCustomerEvent create) => new()
    {
        Address = create.Address,
        City = create.City,
        Region = create.Region,
        PostalCode = create.PostalCode,
        Country = create.Country,
        Phone = create.Phone,
        Fax = create.Fax,
        CompanyName = create.CompanyName,
        ContactName = create.ContactName,
        ContactTitle = create.ContactTitle
    };
}
