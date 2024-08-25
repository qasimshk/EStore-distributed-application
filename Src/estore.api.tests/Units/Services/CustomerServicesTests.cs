namespace estore.api.tests.Units.Services;

using System.Net;
using estore.api.Common;
using estore.api.Mappers;
using estore.api.Models.Aggregates.Customer.ValueObjects;
using estore.api.Persistance.Repositories;
using estore.api.Services;
using estore.api.tests.Fixtures;
using estore.common.Common.Pagination;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;

public class CustomerServicesTests : IClassFixture<MsSqlServerContainerTest>
{
    private readonly CustomerServices _customerServices;
    private readonly CustomerRepository _customerRepository;

    public CustomerServicesTests(MsSqlServerContainerTest msSqlServerContainer)
    {
        _customerRepository = new CustomerRepository(msSqlServerContainer.DbContext);

        var customerMapper = new CustomerMapper();

        var pagedList = new PagedList<CustomerResponse>();

        var unitOfWork = new UnitOfWork(msSqlServerContainer.DbContext);

        var mockValidatorCustomerRequest = new Mock<IValidator<CreateCustomerRequest>>();

        var validationFailure = new ValidationFailure("", "something went wrong");

        mockValidatorCustomerRequest.Setup(x => x.ValidateAsync(It.IsAny<CreateCustomerRequest>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());

        _customerServices = new CustomerServices(
            _customerRepository,
            customerMapper,
            pagedList,
            unitOfWork,
            mockValidatorCustomerRequest.Object);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnTrue_WhenNewCustomerRequestSend()
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
            Country = data.CustomerAddress.Country,
            ContactTitle = data.ContactTitle,
            Fax = data.Fax,
            Phone = data.Phone,
            PostalCode = data.CustomerAddress.PostalCode,
            Region = data.CustomerAddress.Region
        });

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeOfType<CreateCustomerResponse>();

        _customerRepository.GetAll().Should().ContainSingle(x => x.Phone == data.Phone);
    }

    [Fact]
    public async Task GetCustomerByCustomerId_ShouldReturnCustomer_WhenCorrectCustomerIdSend()
    {
        // Arrange
        var customer = _customerRepository.GetAll().First();

        // Act
        var result = await _customerServices.GetCustomerByCustomerId(customer.Id.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeOfType<CustomerResponse>();
    }

    [Fact]
    public async Task GetCustomerByCustomerId_ShouldReturnNotFound_WhenInCorrectCustomerIdSend()
    {
        // Arrange
        var customerId = "Abc01";

        // Act
        var result = await _customerServices.GetCustomerByCustomerId(customerId);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomerByPhoneNumber_ShouldReturnCustomer_WhenCorrectCustomerPhoneNumberSend()
    {
        // Arrange
        var customer = _customerRepository.GetAll().First();

        // Act
        var result = await _customerServices.GetCustomerByPhoneNumber(customer.Phone);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeOfType<CustomerResponse>();
    }

    [Fact]
    public async Task GetCustomerByPhoneNumber_ShouldReturnNotFound_WhenInCorrectPhoneNumberSend()
    {
        // Arrange
        var phoneNumber = "123456789";

        // Act
        var result = await _customerServices.GetCustomerByPhoneNumber(phoneNumber);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnCustomer_WhenValidSearchIsPassed()
    {
        // Arrange
        var searchList = await _customerRepository.GetAll().Select(cus => new SearchCustomerRequest
        {
            CompanyName = cus.CompanyName,
            ContactName = cus.ContactName,
            ContactTitle = cus.ContactTitle
        }).ToListAsync();

        foreach (var search in searchList)
        {
            // Act
            var result = await _customerServices.GetCustomers(search);

            // Assert
            result.Should().NotBeEmpty();

            result.Should().BeOfType<PagedList<CustomerResponse>>();
        }
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnSuccessful_WhenCorrectCustomerIdSend()
    {
        // Arrange
        var customerId = _customerRepository.GetAll().OrderByDescending(x => x.Id).Last().Id.Value;

        // Act
        var result = await _customerServices.DeleteCustomer(customerId);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        _customerRepository.GetAll().Should().NotContain(x => x.Id == new CustomerId(customerId));
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnNotFound_WhenInCorrectCustomerIdSend()
    {
        // Arrange
        var customerId = "Ab123";

        // Act
        var result = await _customerServices.DeleteCustomer(customerId);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
