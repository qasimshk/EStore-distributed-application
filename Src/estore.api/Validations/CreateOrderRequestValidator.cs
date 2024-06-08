namespace estore.api.Validations;

using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Requests;
using FluentValidation;
using Models.Aggregates.Customer.ValueObjects;
using Models.Aggregates.Employee.ValueObjects;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public CreateOrderRequestValidator(IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IEmployeeRepository employeeRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;

        RuleFor(x => x.CustomerId)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (customerId, cancellation) =>
             await CheckCustomerId(customerId))
            .WithMessage(x => $"Customer with this Id '{x.CustomerId}' doesn't exist");

        RuleFor(x => x.EmployeeId)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (employeeId, cancellation) =>
             await CheckEmployeeId(employeeId))
            .WithMessage(x => $"Employee with this Id '{x.EmployeeId}' doesn't exist");

        RuleFor(x => x.OrderDetailsRequest)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .Must(x => x.Count <= 10).WithMessage("Not more than 10 items are allowed")
            .ForEach(orderDetail =>
            {
                orderDetail.Must(od => od.Quantity > 0)
                           .NotEmpty()
                           .NotNull()
                           .WithMessage("Quantity should be more then zero");

                orderDetail.MustAsync(async (orderDetailReq, cancellationToken) =>
                            await CheckProduct(orderDetailReq.ProductId))
                           .WithMessage(x => $"Product with this id doesn't exist")
                           .NotEmpty()
                           .NotNull();

                orderDetail.Must(od => od.UnitPrice > 0)
                           .WithMessage("Unit price should be greater then zero")
                           .NotEmpty()
                           .NotNull();

                orderDetail.NotEmpty()
                           .NotNull();
            })
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.OrderDate)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.RequiredDate)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ShippingDate)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ShipVia)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Freight)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ShipName)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(5, 40);
    }

    private async Task<bool> CheckProduct(int productId)
    {
        var product = await _orderRepository
            .Products(x => x.ProductId == productId);
        return product.Any();
    }

    private async Task<bool> CheckCustomerId(string customerId)
    {
        var customer = await _customerRepository
            .FindByConditionAsync(x => x.Id == new CustomerId(customerId));
        return customer.Any();
    }

    private async Task<bool> CheckEmployeeId(int employeeId)
    {
        var employee = await _employeeRepository
            .FindByConditionAsync(x => x.Id == new EmployeeId(employeeId));
        return employee.Any();
    }
}
