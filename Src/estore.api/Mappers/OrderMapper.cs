namespace estore.api.Mappers;

using estore.api.Abstractions.Mappers;
using estore.api.Models.Aggregates.Orders;
using estore.common.Models.Responses;

public class OrderMapper : IOrderMapper
{
    public OrderResponse Map(Order from) => new()
    {
        CustomerName = from.Customer.ContactName,
        EmployeeName = $"{from.Employee.FirstName} {from.Employee.LastName}",
        Freight = from.Freight,
        OrderDate = from.OrderDate.ToShortDateString(),
        OrderId = from.Id.Value,
        RequiredDate = from.RequiredDate.ToShortDateString(),
        ShippedDate = from.RequiredDate.ToShortDateString(),
        ShipVia = from.ShipVia,
        ShippingAddress = from.ShippingAddress.GetCompleteAddress(),
        OrderDetails = from.OrderDetails.Select(item => new OrderDetailsResponse
        {
            ProductId = item.ProductId,
            CategoryName = item.Item != null ? item.Item.Category.CategoryName : "N/A",
            OrderDetailId = item.Id.Value,
            ProductName = item.Item != null ? item.Item.ProductName : "N/A",
            Discount = item.Discount,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).Distinct().ToList()
    };
}
