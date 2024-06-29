namespace gateway.api.Validator;

using estore.common.Models.Requests;
using FluentValidation;

public class OrderSubmitValidator : AbstractValidator<SubmitOrderRequest>
{
    public OrderSubmitValidator()
    {
        RuleFor(x => x.Customer.CompanyName)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(6, 40);

        RuleFor(x => x.Customer.ContactName)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(3, 30);

        RuleFor(x => x.Customer.ContactTitle)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(5, 30);

        RuleFor(x => x.Customer.Phone)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(5, 30);

        RuleFor(x => x.Order.EmployeeId)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Order.OrderDetailsRequest)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .Must(x => x.Count <= 10).WithMessage("Not more than 10 items are allowed")
            .ForEach(orderDetail =>
            {
                orderDetail.Must(od => od.Quantity > 0)
                           .NotEmpty()
                           .NotNull()
                           .WithMessage("Quantity should be more then zero");

                orderDetail.Must(od => od.ProductId > 0)
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

        RuleFor(x => x.Order.ShipVia)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Order.Freight)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Order.ShipName)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(5, 40);
    }
}
