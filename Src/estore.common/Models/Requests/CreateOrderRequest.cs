namespace estore.common.Models.Requests;

using estore.common.Events;

public class CreateOrderRequest
{
    public string CustomerId { get; set; } = string.Empty;

    public int EmployeeId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public DateTime ShippingDate { get; set; }

    public int ShipVia { get; set; }

    public decimal Freight { get; set; }

    public string ShipName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public List<OrderDetailsRequest> OrderDetailsRequest { get; set; } = [];

    public static CreateOrderRequest Map(CreateOrderEvent @event) => new()
    {
        Address = @event.Address,
        EmployeeId = @event.EmployeeId,
        City = @event.City,
        Region = @event.Region,
        PostalCode = @event.PostalCode,
        Country = @event.Country,
        CustomerId = @event.CustomerId,
        Freight = @event.Freight,
        ShipName = @event.ShipName,
        ShippingDate = @event.ShippingDate,
        ShipVia = @event.ShipVia,
        RequiredDate = @event.RequiredDate,
        OrderDate = @event.OrderDate,
        OrderDetailsRequest = @event.OrderDetailsRequest.Select(x => new OrderDetailsRequest
        {
            Discount = x.Discount,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice,
            Quantity = x.Quantity
        }).ToList(),
    };
}
