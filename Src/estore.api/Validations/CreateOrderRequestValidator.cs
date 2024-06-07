namespace estore.api.Validations;

using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Requests;
using FluentValidation;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderRequestValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(x => x.OrderDetailsRequest)
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
        var product = await _orderRepository.Products(x => x.ProductId == productId);

        return product.Any();
    }
}
