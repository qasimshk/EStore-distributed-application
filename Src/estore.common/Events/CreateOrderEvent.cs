namespace estore.common.Events;
public class CreateOrderEvent : BaseEvent
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

    public List<OrderDetailsEvent> OrderDetailsRequest { get; set; } = [];

    public static CreateOrderEvent Map(SubmitOrderEvent submit, string customerId) => new()
    {
        Address = submit.Address,
        EmployeeId = submit.EmployeeId,
        City = submit.City,
        Region = submit.Region,
        PostalCode = submit.PostalCode,
        Country = submit.Country,
        CorrelationId = submit.CorrelationId,
        CustomerId = customerId,
        Freight = submit.Freight,
        ShipName = submit.ShipName,
        ShippingDate = DateTime.Now.AddDays(5),
        ShipVia = submit.ShipVia,
        RequiredDate = DateTime.Now.AddDays(3),
        OrderDate = DateTime.Now,
        OrderDetailsRequest = submit.OrderDetails
    };

}

