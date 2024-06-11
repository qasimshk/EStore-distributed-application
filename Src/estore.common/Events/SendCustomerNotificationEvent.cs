namespace estore.common.Events;

public class SendCustomerNotificationEvent : BaseEvent
{
    public string CustomerId { get; set; } = string.Empty;
}
