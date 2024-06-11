namespace estore.common.Events;

public class OrderCreatedSuccessfullyEvent : BaseEvent
{
    public int OrderId { get; set; }
}
