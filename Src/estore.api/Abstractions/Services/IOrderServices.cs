namespace estore.api.Abstractions.Services;

using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;

public interface IOrderServices
{
    Task<Result<OrderResponse>> GetOrderByOrderId(int orderId);

    Task<PagedList<OrderResponse>> GetOrderBySearch(SearchOrderRequest searchOrder);

    Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest createOrder);

    Task<Result> DeleteOrder(int orderId);
}
