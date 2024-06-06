namespace estore.api.Services;

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using estore.api.Abstractions.Mappers;
using estore.api.Abstractions.Services;
using estore.api.Common.Pagination;
using estore.api.Common.Results;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Requests;
using estore.api.Models.Responses;
using Models.Aggregates.Customer.ValueObjects;
using Models.Aggregates.Employee.ValueObjects;
using Models.Aggregates.Orders.ValueObjects;

public class OrderServices(IOrderRepository orderRepository,
    IOrderMapper orderMapper,
    IPagedList<OrderResponse> paged) : IOrderServices
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderMapper _orderMapper = orderMapper;
    private readonly IPagedList<OrderResponse> _paged = paged;

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

    public PagedList<OrderResponse> GetOrderBySearch(SearchOrderRequest searchOrder)
    {
        var raw = _orderRepository.GetAll();

        switch (string.IsNullOrEmpty(searchOrder.CustomerId))
        {
            case false when searchOrder.EmployeeId != null:
                raw = raw.Where(x =>
                    x.CustomerId == new CustomerId(searchOrder.CustomerId) &&
                    x.EmployeeId == new EmployeeId(searchOrder.EmployeeId.Value));
                break;
            case false:
                raw = raw.Where(x => x.CustomerId == new CustomerId(searchOrder.CustomerId));
                break;
            default:
            {
                if (searchOrder.EmployeeId != null)
                {
                    raw = raw.Where(x => x.EmployeeId == new EmployeeId(searchOrder.EmployeeId.Value));
                }
                break;
            }
        }

        return _paged.ToPagedList(raw.Select(_orderMapper.Map).AsQueryable(), searchOrder.PageNumber, searchOrder.PageSize);
    }
}
