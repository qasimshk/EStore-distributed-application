namespace gateway.api.Services;

using estore.common.Models.Requests;

public interface IEStoreServices
{
    Task<IResult> GetCustomerById(string customerId);

    Task<IResult> GetCustomerBySearch(SearchCustomerRequest search, HttpContext http);

    Task<IResult> GetOrderByOrderId(int orderId);

    Task<IResult> GetOrderBySearch(SearchOrderRequest search, HttpContext http);

    Task<IResult> SubmitOrder(SubmitOrderRequest submit);

    Task<IResult> GetOrderState(Guid correlationId);

    Task<IResult> RefundOrder(Guid correlationId);

    Task<IResult> RemoveOrder(Guid correlationId);
}
