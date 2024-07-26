namespace estore.common.Events;

public class DeleteOrderWithCustomerEvent : BaseEvent
{
    public int OrderId { get; set; }

    public string CustomerId { get; set; } = string.Empty;
}
