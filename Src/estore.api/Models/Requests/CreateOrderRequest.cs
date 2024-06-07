namespace estore.api.Models.Requests;

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
}
