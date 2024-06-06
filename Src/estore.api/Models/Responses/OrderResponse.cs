namespace estore.api.Models.Responses;

public class OrderResponse : CustomerOrderResponse
{
    public List<OrderDetailsResponse> OrderDetails { get; set; } = [];
}
