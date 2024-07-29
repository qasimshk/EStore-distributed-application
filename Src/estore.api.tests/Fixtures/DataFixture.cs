namespace estore.api.tests.Fixtures;

using Bogus;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Aggregates.Orders.Entities;
using static Bogus.DataSets.Name;

public class CustomerFaker : Faker<Customer>
{
    public static Faker<Customer> GetData() => (CustomerFaker)new CustomerFaker()
            .CustomInstantiator(faker => Customer.Create(
                faker.Company.CompanyName(),
                faker.Name.FullName(faker.PickRandom<Gender>()),
                faker.Name.JobTitle(),
                faker.Phone.PhoneNumber(),
                faker.Phone.PhoneNumber(),
                AddressFaker.GetData().Generate(1).Single()));

    [Fact]
    public void GetData_WhenInvoke_ReturnCustomerType()
    {
        // Act
        var customer = CustomerFaker.GetData().Generate(1).Single();

        // Assert
        customer.Should().BeOfType<Customer>();
    }
}

public class AddressFaker : Faker<Addresses>
{
    public static Faker<Addresses> GetData() => (AddressFaker)new AddressFaker()
        .CustomInstantiator(faker => Addresses.Create(
                        faker.Address.FullAddress(),
                        faker.Address.City(),
                        faker.Address.State(),
                        faker.Address.ZipCode(),
                        faker.Address.Country()));

    [Fact]
    public void GetData_WhenInvoke_ReturnAddressesType()
    {
        // Act
        var address = AddressFaker.GetData().Generate(1).Single();

        // Assert
        address.Should().BeOfType<Addresses>();
    }
}

public class OrderFaker : Faker<Order>
{
    public static Faker<Order> GetData(Customer customer) => (OrderFaker)new OrderFaker()
        .CustomInstantiator(faker => Order.Create(
            customer,
            EmployeeFaker.GetData().Generate(1).Single(),
            DateTime.Now,
            DateTime.Now.AddDays(5),
            DateTime.Now.AddDays(10),
            faker.Random.Number(10, 90),
            faker.Random.Decimal(10, 20),
            faker.Company.CompanyName(),
            AddressFaker.GetData().Generate(1).Single()));

    [Fact]
    public void GetData_WhenInvoke_ReturnOrderType()
    {
        // Arrange
        var customer = CustomerFaker.GetData().Generate(1).Single();

        // Act
        var order = OrderFaker.GetData(customer).Generate(1).Single();

        // Assert
        order.Should().BeOfType<Order>();
    }
}

public class OrderDetailsFaker : Faker<OrderDetail>
{
    public static Faker<OrderDetail> GetData(Order order) => (OrderDetailsFaker)new OrderDetailsFaker()
        .CustomInstantiator(faker => OrderDetail.Create(order,
            faker.Random.Number(1,10),
            faker.Random.Decimal(10, 99),
            faker.Random.Number(1, 10),
            faker.Random.Double(10, 99)));

    [Fact]
    public void GetData_WhenInvoke_ReturnOrderDetailsType()
    {
        // Arrange
        var customer = CustomerFaker.GetData().Generate(1).Single();
        var order = OrderFaker.GetData(customer).Generate(1).Single();

        // Act
        var orderDetails = OrderDetailsFaker.GetData(order).Generate(1).Single();

        // Assert
        orderDetails.Should().BeOfType<OrderDetail>();
    }
}

public class EmployeeFaker : Faker<Employee>
{
    public static Faker<Employee> GetData() => (EmployeeFaker)new EmployeeFaker()
        .CustomInstantiator(faker => new Employee(new EmployeeId(faker.Random.Number(1, 20)),
            faker.Name.JobTitle(),
            faker.Name.FirstName(),
            faker.Name.LastName(),
            faker.Name.JobTitle(),
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
    public void GetData_WhenInvoke_ReturnEmployeeType()
    {
        // Act
        var employee = EmployeeFaker.GetData().Generate(1).Single();

        // Assert
        employee.Should().BeOfType<Employee>();
    }
}
