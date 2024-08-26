namespace estore.api.tests.Fixtures;

using Bogus;
using static Bogus.DataSets.Name;

public class SupplierFaker : Faker<Supplier>
{
    public static Faker<Supplier> GetData() => new Faker<Supplier>()
        .CustomInstantiator(faker => Supplier.Create(
            faker.Company.CompanyName(),
            faker.Name.FindName(),
            "Sale",
            faker.Phone.PhoneNumber(),
            faker.Phone.PhoneNumber(),
            "Test",
            AddressFaker.GetData().Generate(1).Single()));

    [Fact]
    public void GetDataWhenInvokeReturnCustomerType()
    {
        // Act
        var customer = GetData().Generate(1).Single();

        // Assert
        customer.Should().BeOfType<Supplier>();
    }
}

public class CategoryFaker : Faker<Category>
{
    public static Faker<Category> GetData() => new Faker<Category>()
        .CustomInstantiator(faker => Category.Create(
            faker.Commerce.Categories(1)[0],
            faker.Commerce.Categories(1)[0]));

    [Fact]
    public void GetDataWhenInvokeReturnCustomerType()
    {
        // Act
        var customer = GetData().Generate(1).Single();

        // Assert
        customer.Should().BeOfType<Category>();
    }
}

public class ProductFaker : Faker<Product>
{
    public static Faker<Product> GetData(int categoryId, int supplierId) => new Faker<Product>()
        .CustomInstantiator(faker => Product.Create(faker.Commerce.ProductName(),
            supplierId,
            categoryId,
            faker.Random.Number(1, 9).ToString(),
            faker.Random.Decimal(10, 20),
            faker.Random.Number(10, 90),
            faker.Random.Number(1, 10),
            faker.Random.Number(5, 9),
            true));

    [Fact]
    public void GetDataWhenInvokeReturnCustomerType()
    {
        // Act
        var customer = GetData(1,1).Generate(1).Single();

        // Assert
        customer.Should().BeOfType<Product>();
    }
}

public class CustomerFaker : Faker<Customer>
{
    public static Faker<Customer> GetData() => (CustomerFaker)new CustomerFaker()
            .CustomInstantiator(faker => Customer.Create(
                faker.Company.CompanyName(),
                faker.Name.FullName(faker.PickRandom<Gender>()),
                "Software Engineer",
                faker.Phone.PhoneNumber(),
                faker.Phone.PhoneNumber(),
                AddressFaker.GetData().Generate(1).Single()));

    [Fact]
    public void GetDataWhenInvokeReturnCustomerType()
    {
        // Act
        var customer = GetData().Generate(1).Single();

        // Assert
        customer.Should().BeOfType<Customer>();
    }
}

public class AddressFaker : Faker<Addresses>
{
    public static Faker<Addresses> GetData() => (AddressFaker)new AddressFaker()
        .CustomInstantiator(faker => Addresses.Create(
                        "Test Address",
                        "Test City",
                        faker.Address.State(),
                        faker.Address.ZipCode(),
                        "UK"));

    [Fact]
    public void GetDataWhenInvokeReturnAddressesType()
    {
        // Act
        var address = GetData().Generate(1).Single();

        // Assert
        address.Should().BeOfType<Addresses>();
    }
}

public class OrderFaker : Faker<Order>
{
    public static Faker<Order> GetData(Customer customer, Employee employee) => (OrderFaker)new OrderFaker()
        .CustomInstantiator(faker => Order.Create(
            customer,
            employee,
            DateTime.Now,
            DateTime.Now.AddDays(5),
            DateTime.Now.AddDays(10),
            faker.Random.Number(10, 90),
            faker.Random.Decimal(10, 20),
            faker.Company.CompanyName(),
            AddressFaker.GetData().Generate(1).Single()));

    [Fact]
    public void GetDataWhenInvokeReturnOrderType()
    {
        // Arrange
        var customer = CustomerFaker.GetData().Generate(1).Single();
        var employee = EmployeeFaker.GetData().Generate(1).Single();

        // Act
        var order = GetData(customer, employee).Generate(1).Single();

        // Assert
        order.Should().BeOfType<Order>();
    }
}

public class OrderDetailsFaker : Faker<OrderDetail>
{
    public static Faker<OrderDetail> GetData(Order order, int productId) => (OrderDetailsFaker)new OrderDetailsFaker()
        .CustomInstantiator(faker => OrderDetail.Create(order,
            productId,
            faker.Random.Decimal(10, 99),
            faker.Random.Number(1, 10),
            faker.Random.Double(10, 99)));

    [Fact]
    public void GetDataWhenInvokeReturnOrderDetailsType()
    {
        // Arrange
        var customer = CustomerFaker.GetData().Generate(1).Single();
        var employee = EmployeeFaker.GetData().Generate(1).Single();
        var order = OrderFaker.GetData(customer, employee).Generate(1).Single();

        // Act
        var orderDetails = GetData(order, 1).Generate(1).Single();

        // Assert
        orderDetails.Should().BeOfType<OrderDetail>();
    }
}

public class EmployeeFaker : Faker<Employee>
{
    private static int GetValue() => Math.Abs(new Guid(Guid.NewGuid().ToString()).GetHashCode());

    public static Faker<Employee> GetData() => (EmployeeFaker)new EmployeeFaker()
        .CustomInstantiator(faker => new Employee(new EmployeeId(GetValue()),
            "Mr",
            faker.Name.FirstName(),
            faker.Name.LastName(),
            "Test",
            faker.Person.DateOfBirth,
            DateTime.Now,
            faker.Phone.PhoneNumber(),
            "ext",
            faker.Random.Bytes(90),
            "Notes",
            1,
            "TestPath",
            AddressFaker.GetData().Generate(1).Single()));


    [Fact]
    public void GetDataWhenInvokeReturnEmployeeType()
    {
        // Act
        var employee = GetData().Generate(1).Single();

        // Assert
        employee.Should().BeOfType<Employee>();
    }
}
