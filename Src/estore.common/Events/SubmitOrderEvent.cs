namespace estore.common.Events;

using System.Collections.Generic;
using estore.common.Models.Requests;

public class SubmitOrderEvent : BaseEvent
{
    public CreateCustomerEvent CreateCustomer { get; set; } = new();

    public int EmployeeId { get; set; }

    public int ShipVia { get; set; }

    public decimal Freight { get; set; }

    public string ShipName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public List<OrderDetailsEvent> OrderDetails { get; set; } = [];

    public static SubmitOrderEvent Map(SubmitOrderRequest request) => new()
    {
        Address = request.Address,
        City = request.City,
        Region = request.Region,
        PostalCode = request.PostalCode,
        Country = request.Country,
        OrderDetails = request.OrderDetails,
        Freight = request.Freight,
        ShipName = request.ShipName,
        CorrelationId = request.CorrelationId,
        CreateCustomer = request.CreateCustomer,
        EmployeeId = request.EmployeeId,
        ShipVia = request.ShipVia,
    };
}
