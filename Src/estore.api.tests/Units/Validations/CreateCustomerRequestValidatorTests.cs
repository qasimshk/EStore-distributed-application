namespace estore.api.tests.Units.Validations;

using System.Linq.Expressions;
using estore.api.Models.Aggregates.Customer;
using estore.api.tests.Fixtures;
using estore.api.Validations;
using estore.common.Models.Requests;
using Moq;

public class CreateCustomerRequestValidatorTests
{
    private readonly CreateCustomerRequestValidator _validator;
    private readonly Mock<ICustomerRepository> _mockRepository;

    public CreateCustomerRequestValidatorTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _validator = new CreateCustomerRequestValidator(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateCustomerRequestValidator_ShouldReturnValid_WhenNewCustomerRequestSend()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync([]);

        var fakeData = CustomerFaker.GetData().Generate(1).Single();

        var request = new CreateCustomerRequest
        {
            Address = fakeData.CustomerAddress.GetCompleteAddress(),
            City = fakeData.CustomerAddress.City,
            Region = fakeData.CustomerAddress.Region,
            CompanyName = fakeData.CompanyName,
            ContactName = fakeData.ContactName,
            ContactTitle = fakeData.ContactTitle,
            Country = fakeData.CustomerAddress?.Country,
            Fax = fakeData.Fax,
            Phone = "07234134463",
            PostalCode = fakeData.CustomerAddress?.PostalCode
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().HaveCount(0);
    }

    [Fact]
    public async Task CreateCustomerRequestValidator_ShouldReturnInValid_WhenNewCustomerRequestSendWithExistingPhoneNumber()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync([.. CustomerFaker.GetData().Generate(1)]);

        var fakeData = CustomerFaker.GetData().Generate(1).Single();

        var request = new CreateCustomerRequest
        {
            Address = fakeData.CustomerAddress.GetCompleteAddress(),
            City = fakeData.CustomerAddress.City,
            Region = fakeData.CustomerAddress.Region,
            CompanyName = fakeData.CompanyName,
            ContactName = fakeData.ContactName,
            ContactTitle = fakeData.ContactTitle,
            Country = fakeData.CustomerAddress?.Country,
            Fax = fakeData.Fax,
            Phone = fakeData.Phone,
            PostalCode = fakeData.CustomerAddress?.PostalCode
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateCustomerRequestValidator_ShouldReturnInValid_WhenNewCustomerRequestSendWithInvalidRecords()
    {
        // Arrange
        var fakeData = CustomerFaker.GetData().Generate(1).Single();

        var request = new CreateCustomerRequest
        {
            Address = fakeData.CustomerAddress.GetCompleteAddress(),
            City = fakeData.CustomerAddress.City,
            Region = fakeData.CustomerAddress.Region,
            CompanyName = "Asqef",
            ContactName = "AB",
            ContactTitle = "Aqrf",
            Country = fakeData.CustomerAddress?.Country,
            Fax = fakeData.Fax,
            Phone = fakeData.Phone,
            PostalCode = fakeData.CustomerAddress?.PostalCode
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(3);
    }
}
