namespace estore.common.Events;

public class OrderStateEvent : BaseEvent
{
    public string CurrentState { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? FailedOn { get; set; }
    public string? ErrorMessage { get; set; }
}
