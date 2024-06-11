namespace estore.common.Models.Responses;

public class OrderSubmittedResponse
{
    public Guid ReferenceNumber { get; set; }

    public string SubmittedDate { get; set; } = string.Empty;
}
