namespace estore.api.Models.Responses;

public class OrderResponse
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string EmployeeName { get; set; } = string.Empty;

    public string OrderDate { get; set; } = string.Empty;

    public string RequiredDate { get; set; } = string.Empty;

    public string ShippedDate { get; set;} = string.Empty;

    public int ShipVia { get; set; }

    public decimal Freight { get; set; }

    public string ShippingAddress { get; set; } = string.Empty;
}

