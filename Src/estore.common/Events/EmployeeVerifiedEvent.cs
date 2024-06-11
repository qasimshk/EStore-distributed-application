namespace estore.common.Events;

public class EmployeeVerifiedEvent : BaseEvent
{
    public int EmployeeId { get; set; }
    public bool Verified { get; set; }
}
