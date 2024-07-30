namespace estore.api.tests.Units.Services;

using estore.api.Abstractions.Services;
using estore.api.Common;
using estore.api.Mappers;
using estore.api.Persistance.Repositories;
using estore.api.Services;
using estore.api.tests.Fixtures;
using estore.common.Common.Pagination;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using FluentValidation.Results;
using Moq;

public class CustomerServicesTests : IClassFixture<MsSqlServerContainerTest>
{
    private readonly ICustomerServices _customerServices;

    public CustomerServicesTests(MsSqlServerContainerTest msSqlServerContainer)
    {
        var customerRepository = new CustomerRepository(msSqlServerContainer.GetDbContext());

        var customerMapper = new CustomerMapper();

        var pagedList = new PagedList<CustomerResponse>();

        var unitOfWork = new UnitOfWork(msSqlServerContainer.GetDbContext());

        var mockValidatorCustomerRequest = new Mock<IValidator<CreateCustomerRequest>>();

        var validationFailure = new ValidationFailure("", "something went wrong");

        mockValidatorCustomerRequest.Setup(x => x.ValidateAsync(It.IsAny<CreateCustomerRequest>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());

        _customerServices = new CustomerServices(
            customerRepository,
            customerMapper,
            pagedList,
            unitOfWork,
            mockValidatorCustomerRequest.Object);
    }

    [Fact]
    public async Task TestOne()
    {
        // Arrange
        var data = CustomerFaker.GetData().Generate(1).Single();

        // Act
        var result = await _customerServices.CreateCustomer(new CreateCustomerRequest
        {
            Address = data.CustomerAddress.GetCompleteAddress(),
            City = data.CustomerAddress.City,
            CompanyName = data.CompanyName,
            ContactName = data.ContactName,
            Country = data.CompanyName,
            ContactTitle = data.ContactTitle,
            Fax = data.Fax,
            Phone = data.Phone,
            PostalCode = data.CustomerAddress.PostalCode,
            Region = data.CustomerAddress.Region
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<CreateCustomerResponse>();
    }
}
