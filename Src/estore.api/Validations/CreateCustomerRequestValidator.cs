namespace estore.api.Validations;

using estore.api.Models.Aggregates.Customer;
using estore.common.Models.Requests;
using FluentValidation;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerRequestValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(x => x.CompanyName)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(6, 40);

        RuleFor(x => x.ContactName)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(3, 30);

        RuleFor(x => x.ContactTitle)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(5, 30);

        RuleFor(x => x.Phone)
            .Cascade(cascadeMode: CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (phone, cancellation) => await CheckCustomerPhoneNumber(phone))
            .WithMessage(x => $"Customer with this phone number '{x.Phone}' already exist");
    }

    private async Task<bool> CheckCustomerPhoneNumber(string phoneNumber)
    {
        var customer = await _customerRepository.FindByConditionAsync(x => x.Phone == phoneNumber);

        return !customer.Any();
    }
}
