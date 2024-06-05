namespace estore.api.Mappers;

using estore.api.Abstractions.Mappers;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Responses;

public class OrderMapper : IOrderMapper
{
    public OrderResponse Map(Order from) => new()
    {
        CustomerName = from.Customer.ContactName,
        EmployeeName = "dfdsfsd",//$"{from.Employee.FirstName} {from.Employee.LastName}",
        Freight = from.Freight,
        OrderDate = from.OrderDate.ToShortDateString(),
        OrderId = from.Id.Value,
        RequiredDate = from.RequiredDate.ToShortDateString(),
        ShippedDate = from.RequiredDate.ToShortDateString(),
        ShipVia = from.ShipVia,
        ShippingAddress = from.ShippingAddress.GetCompleteAddress()
    };
}
