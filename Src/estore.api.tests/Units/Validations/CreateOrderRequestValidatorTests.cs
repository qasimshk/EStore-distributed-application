namespace estore.api.tests.Units.Validations;

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
        var fakeEmployee = EmployeeFaker.GetData().Generate(1).Single();
        var fakeOrder = OrderFaker.GetData(fakeCustomer, fakeEmployee).Generate(1).Single();
        var fakeOrderDetails = OrderDetailsFaker.GetData(fakeOrder, 1).Generate(1).Single();

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

    [Fact]
    public async Task CreateOrderRequestValidator_ShouldReturnInValid_WhenNewOrderRequestSendWithNoReferenceDataExist()
    {
        // Arrange
        var fakeCustomer = CustomerFaker.GetData().Generate(1).Single();
        var fakeEmployee = EmployeeFaker.GetData().Generate(1).Single();
        var fakeOrder = OrderFaker.GetData(fakeCustomer, fakeEmployee).Generate(1).Single();
        var fakeOrderDetails = OrderDetailsFaker.GetData(fakeOrder, 1).Generate(1).Single();

        _mockCustomerRepository.Setup(x => x.FindByConditionAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync([]);

        _mockEmployeeRepository.Setup(x => x.FindByConditionAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync([]);

        _mockOrderRepository.Setup(x => x.FindByConditionAsync(It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync([]);

        _mockOrderRepository.Setup(x => x.Products(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync([]);

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
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(3);

        result.Errors.Select(e => e.PropertyName).Should().Contain(["CustomerId", "EmployeeId", "OrderDetailsRequest[0]"]);
    }
}
