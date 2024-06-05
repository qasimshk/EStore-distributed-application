namespace estore.api.Mappers;

using estore.api.Abstractions.Mappers;
using estore.api.Models.Aggregates.Customer;
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
        CustomerOrders = from.Orders.Select(ord => new OrderResponse
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
}
