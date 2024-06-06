namespace estore.api.Mappers;

using estore.api.Abstractions.Mappers;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Requests;
using estore.api.Models.Responses;

public class CustomerMapper : ICustomerMapper
{
    public CustomerResponse Map(Customer from) => new()
    {
        CustomerId = from.Id.Value,
        CompanyName = from.CompanyName,
        Address = from.CustomerAddress.GetCompleteAddress(),
        ContactName = from.ContactName,
        ContactTitle = from.ContactTitle,
        Fax = from.Fax,
        Phone = from.Phone,
        CustomerOrders = from.Orders.Select(ord => new CustomerOrderResponse
        {
            CustomerName = ord.Customer.ContactName,
            EmployeeName = $"{ord.Employee.TitleOfCourtesy} {ord.Employee.FirstName}, {ord.Employee.LastName}",
            Freight = ord.Freight,
            OrderDate = ord.OrderDate.ToShortDateString(),
            OrderId = ord.Id.Value,
            RequiredDate = ord.RequiredDate.ToShortDateString(),
            ShippedDate = ord.ShippedDate.HasValue ? ord.ShippedDate.Value.ToShortDateString() : null,
            ShippingAddress = ord.ShippingAddress.GetCompleteAddress(),
            ShipVia = ord.ShipVia
        }).ToList()
    };

    public Customer Map(CreateCustomerRequest from)
    {
        var address = Addresses.Create(from.Address,
            from.City, from.Region, from.PostalCode, from.Country);

        return Customer.Create(from.CompanyName, from.ContactName,
            from.ContactTitle, from.Phone, from.Fax, address);
    }

    public CreateCustomerResponse Mapper(Customer customer) => new CreateCustomerResponse
    {
        CustomerAddress = customer.CustomerAddress.GetCompleteAddress(),
        CompanyName = customer.CompanyName,
        ContactTitle = customer.ContactTitle,
        ContactName = customer.ContactName,
        CustomerID = customer.Id.Value,
        Fax = customer.Fax,
        Phone = customer.Phone
    };
}
