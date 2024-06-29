namespace estore.common.Events;

public class OrderInformationEvent : BaseEvent
{
    public string Message { get; set; } = string.Empty;
}
