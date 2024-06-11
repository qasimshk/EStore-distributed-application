namespace estore.common.Events;

public class OrderDetailsEvent
{
    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }
}
