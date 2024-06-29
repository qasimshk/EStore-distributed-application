namespace estore.common.Events;

public class PaymentInformationEvent : BaseEvent
{
    public string Message { get; set; } = string.Empty;
}
