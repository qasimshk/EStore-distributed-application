namespace estore.common.Events;

public class FailedEvent : BaseEvent
{
    public string ConsumerName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
