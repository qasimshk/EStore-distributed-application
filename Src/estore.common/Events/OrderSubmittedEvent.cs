namespace estore.common.Events;

public class OrderSubmittedEvent : BaseEvent
{
    public string OrderState { get; set; } = string.Empty;
    public string SubmittedDate { get; set; } = string.Empty;
}
