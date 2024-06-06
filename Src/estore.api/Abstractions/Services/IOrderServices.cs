namespace estore.api.Abstractions.Services;

using estore.api.Common.Results;
using estore.api.Models.Responses;

public interface IOrderServices
{
    Task<Result<CustomerOrderResponse>> GetOrderByOrderId(int orderId);
}
