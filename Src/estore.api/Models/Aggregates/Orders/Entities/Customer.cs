namespace estore.api.Models.Aggregates.Orders.Entities;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Orders.ValueObjects;

public class Customer : Entity<CustomerId>
{
    private readonly List<Order> _orders = [];

    public string CompanyName { get; } = string.Empty;

    public string ContactName { get; }

    public string ContactTitle { get; }

    public string Phone { get; set; }

    public string? Fax { get; set; }

    public Addresses CustomerAddress { get; }

    public IReadOnlyList<Order> Orders => _orders.AsReadOnly();

    private Customer() { }

    private Customer(CustomerId customerId,
        string companyName,
        string contactName,
        string contactTitle,
        string phone,
        string fax,
        Addresses customerAddress) : base(customerId)
    {
        CompanyName = companyName;
        ContactName = contactName;
        ContactTitle = contactTitle;
        Phone = phone;
        Fax = fax;
        CustomerAddress = customerAddress;
    }

    public static Customer Create(string companyName,
        string contactName,
        string contactTitle,
        string phone,
        string fax,
        Addresses customerAddress) => new(CustomerId.CreateUnique(),
            companyName,
            contactName,
            contactTitle,
            phone,
            fax,
            customerAddress);
}
