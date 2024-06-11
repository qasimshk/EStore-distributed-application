namespace estore.api.Services;

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using estore.api.Abstractions.Mappers;
using estore.api.Abstractions.Services;
using estore.api.Common;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Aggregates.Orders.Entities;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using Models.Aggregates.Customer.ValueObjects;
using Models.Aggregates.Employee.ValueObjects;
using Models.Aggregates.Orders.ValueObjects;

public class OrderServices(IOrderRepository orderRepository,
    IOrderMapper orderMapper,
    IPagedList<OrderResponse> paged,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IValidator<CreateOrderRequest> validator,
    IUnitOfWork unitOfWork) : IOrderServices
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderMapper _orderMapper = orderMapper;
    private readonly IPagedList<OrderResponse> _paged = paged;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IValidator<CreateOrderRequest> _validator = validator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest createOrder)
    {
        var result = await _validator.ValidateAsync(createOrder);

        if (result.IsValid)
        {
            var customer = await _customerRepository
            .FindByConditionAsync(cus => cus.Id == new CustomerId(createOrder.CustomerId));

            var employee = await _employeeRepository
                .FindByConditionAsync(emp => emp.Id == new EmployeeId(createOrder.EmployeeId));

            var shippingAddress = Addresses.Create(createOrder.Address, createOrder.City, createOrder.Region,
                createOrder.PostalCode, createOrder.Country);

            var order = Order.Create(customer.Single(), employee.Single(), createOrder.OrderDate, createOrder.RequiredDate,
                createOrder.ShippingDate, createOrder.ShipVia, createOrder.Freight, createOrder.ShipName,
                shippingAddress);

            var orderDetails = createOrder.OrderDetailsRequest.Select(od =>
                OrderDetail.Create(order, od.ProductId, od.UnitPrice, od.Quantity, od.Discount));

            _orderRepository.Add(order);

            _orderRepository.AddOrderDetails(orderDetails.ToList());

            var resp = await _unitOfWork.CompleteAsync();

            if (resp > 0)
            {
                var createdOrder = await _orderRepository.FindByConditionAsync(x => x.Id == new OrderId(order.Id.Value));

                return Result<OrderResponse>.SuccessResult(_orderMapper.Map(createdOrder.First()));
            }
            return Result<OrderResponse>.FailedResult("Order was not able to be created", HttpStatusCode.BadRequest);
        }
        return Result<OrderResponse>.FailedResult(result.Errors.Select(x => x.ErrorMessage).First(), HttpStatusCode.BadRequest);
    }

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
        return _paged.ToPagedList(raw.Distinct().Select(_orderMapper.Map).AsQueryable(), searchOrder.PageNumber, searchOrder.PageSize);
    }
}
