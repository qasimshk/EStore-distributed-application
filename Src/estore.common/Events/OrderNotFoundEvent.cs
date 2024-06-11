namespace estore.common.Events;

public class OrderNotFoundEvent : BaseEvent
{
    public string Message { get; set; } = string.Empty;
}
