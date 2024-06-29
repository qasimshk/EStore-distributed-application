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
        Address = request.Order.Address,
        City = request.Order.City,
        Region = request.Order.Region,
        PostalCode = request.Order.PostalCode,
        Country = request.Order.Country,
        OrderDetails = request.Order.OrderDetailsRequest.Select(x => new OrderDetailsEvent
        {
            Discount = x.Discount,
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice
        }).ToList(),
        Freight = request.Order.Freight,
        ShipName = request.Order.ShipName,
        CorrelationId = request.CorrelationId,
        CreateCustomer = new CreateCustomerEvent
        {
            Address = request.Customer.Address,
            City = request.Customer.City,
            Region = request.Customer.Region,
            PostalCode = request.Customer.PostalCode,
            Country = request.Customer.Country,
            CompanyName = request.Customer.CompanyName,
            ContactName = request.Customer.ContactName,
            ContactTitle = request.Customer.ContactTitle,
            CorrelationId = request.CorrelationId,
            Fax = request.Customer.Fax,
            Phone = request.Customer.Phone
        },
        EmployeeId = request.Order.EmployeeId,
        ShipVia = request.Order.ShipVia,
    };
}
