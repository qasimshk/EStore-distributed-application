namespace estore.common.Events;

public class PaymentStateEvent : BaseEvent
{
    public string CurrentState { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? FailedOn { get; set; }
    public string? ErrorMessage { get; set; }
}
