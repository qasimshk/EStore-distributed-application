namespace estore.api.Services;

using System.Net;
using System.Threading.Tasks;
using estore.api.Abstractions.Mappers;
using estore.api.Abstractions.Services;
using estore.api.Common.Results;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Responses;
using Models.Aggregates.Orders.ValueObjects;

public class OrderServices(IOrderRepository orderRepository,
    IOrderMapper orderMapper) : IOrderServices
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderMapper _orderMapper = orderMapper;

    public async Task<Result<OrderResponse>> GetOrderByOrderId(int orderId)
    {
        var order = await _orderRepository
            .FindByConditionAsync(x => x.Id == new OrderId(orderId));

        return order.Any() ?
            Result<OrderResponse>
                .SuccessResult(_orderMapper.Map(order.SingleOrDefault()!)) :
            Result<OrderResponse>
                .FailedResult("Order not found with this Id", HttpStatusCode.NotFound);

    }
}
