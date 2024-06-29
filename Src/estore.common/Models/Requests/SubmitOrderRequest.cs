namespace estore.common.Models.Requests;

public class SubmitOrderRequest
{
    public Guid CorrelationId { get; } = Guid.NewGuid();

    public CreateCustomerRequest Customer { get; set; } = new();

    public PlaceOrderRequest Order { get; set; } = new();
}
