namespace estore.common.Events;

public class CustomerCreatedSuccessfullyEvent : BaseEvent
{
    public string CustomerId { get; set; } = string.Empty;
}
