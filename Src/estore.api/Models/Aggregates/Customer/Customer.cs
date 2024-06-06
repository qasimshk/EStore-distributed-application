namespace estore.api.Models.Aggregates.Customer;

using estore.api.Common.Models;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer.ValueObjects;
using estore.api.Models.Aggregates.Orders;

public sealed class Customer : AggregateRoot<CustomerId>
{
    private readonly List<Order> _orders = [];

    public string CompanyName { get; } = string.Empty;

    public string ContactName { get; }

    public string ContactTitle { get; }

    public string Phone { get; }

    public string? Fax { get; }

    public Addresses CustomerAddress { get; }

    public IReadOnlyList<Order> Orders => _orders.AsReadOnly();

    protected Customer() { }

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
