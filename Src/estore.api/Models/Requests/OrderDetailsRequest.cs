namespace estore.api.Models.Requests;

public class OrderDetailsRequest
{
    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }
}
