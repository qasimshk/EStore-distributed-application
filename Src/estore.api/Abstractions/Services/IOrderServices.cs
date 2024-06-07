namespace estore.api.Abstractions.Services;

using estore.api.Common.Pagination;
using estore.api.Common.Results;
using estore.api.Models.Requests;
using estore.api.Models.Responses;

public interface IOrderServices
{
    Task<Result<OrderResponse>> GetOrderByOrderId(int orderId);

    PagedList<OrderResponse> GetOrderBySearch(SearchOrderRequest searchOrder);

    Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest createOrder);
}
