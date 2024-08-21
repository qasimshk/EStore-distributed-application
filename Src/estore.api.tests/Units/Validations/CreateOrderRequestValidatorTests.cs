namespace estore.api.tests.Units.Validations;

using System.Linq.Expressions;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Orders;
using estore.api.tests.Fixtures;
using estore.api.Validations;
using estore.common.Models.Requests;
using Moq;

public class CreateOrderRequestValidatorTests
{
    private readonly CreateOrderRequestValidator _validator;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    public CreateOrderRequestValidatorTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockOrderRepository = new Mock<IOrderRepository>();

        _validator = new CreateOrderRequestValidator(
            _mockOrderRepository.Object,
            _mockCustomerRepository.Object,
            _mockEmployeeRepository.Object);
    }

    [Fact]
    public async Task CreateOrderRequestValidator_ShouldReturnValid_WhenNewOrderRequestSend()
    {
        // Arrange
        var fakeCustomer = CustomerFaker.GetData().Generate(1).Single();
        var fakeOrder = OrderFaker.GetData(fakeCustomer).Generate(1).Single();
        var fakeOrderDetails = OrderDetailsFaker.GetData(fakeOrder).Generate(1).Single();

        _mockCustomerRepository.Setup(x => x.FindByConditionAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync([fakeCustomer]);

        _mockEmployeeRepository.Setup(x => x.FindByConditionAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync([.. EmployeeFaker.GetData().Generate(1)]);

        _mockOrderRepository.Setup(x => x.FindByConditionAsync(It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync([fakeOrder]);

        _mockOrderRepository.Setup(x => x.Products(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(
             [
                new Product()
             ]);

        var request = new CreateOrderRequest
        {
            Address = fakeCustomer.CustomerAddress.GetCompleteAddress(),
            City = fakeCustomer.CustomerAddress.City,
            Region = fakeCustomer.CustomerAddress.Region,
            Country = fakeCustomer.CustomerAddress.Country,
            CustomerId = fakeCustomer.Id.Value,
            EmployeeId = fakeOrder.EmployeeId.Value,
            Freight = fakeOrder.Freight,
            OrderDate = fakeOrder.OrderDate,
            OrderDetailsRequest =
            [
                new OrderDetailsRequest
                {
                    Discount = fakeOrderDetails.Discount,
                    ProductId = fakeOrderDetails.ProductId,
                    Quantity = fakeOrderDetails.Quantity,
                    UnitPrice = fakeOrderDetails.UnitPrice,
                }
            ],
            PostalCode = fakeCustomer.CustomerAddress.PostalCode,
            RequiredDate = fakeOrder.RequiredDate,
            ShipName = fakeOrder.ShipName,
            ShippingDate = fakeOrder.ShippedDate!.Value,
            ShipVia = fakeOrder.ShipVia
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().BeEmpty();
    }
}
