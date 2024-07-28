namespace estore.api.tests.Fixtures;

using Bogus;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Customer.ValueObjects;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Aggregates.Orders.Entities;
using estore.api.Models.Aggregates.Orders.ValueObjects;
using static Bogus.DataSets.Name;

public static class DataFixture
{
    public static Faker<Customer> GetCustomer() => new Faker<Customer>()
            .RuleFor(c => c.Id, (faker, t) => new CustomerId(faker.Random.AlphaNumeric(7)))
            .RuleFor(c => c.CompanyName, (faker, t) => faker.Company.CompanyName())
            .RuleFor(c => c.ContactName, (faker, t) => faker.Name.FullName(faker.PickRandom<Gender>()))
            .RuleFor(c => c.ContactTitle, (faker, t) => faker.Name.JobTitle())
            .RuleFor(c => c.Phone, (faker, t) => faker.Phone.PhoneNumber())
            .RuleFor(c => c.Fax, (faker, t) => faker.Phone.PhoneNumber())
            .RuleFor(c => c.Orders, (faker, t) => GetOrders(t.Id.Value).Generate(3))
            .RuleFor(c => c.CustomerAddress, (faker, t) =>
                    Addresses.Create(
                        faker.Address.FullAddress(),
                        faker.Address.City(),
                        faker.Address.State(),
                        faker.Address.ZipCode(),
                        faker.Address.Country()
                    ));

    public static Faker<Order> GetOrders(string? customerId) => new Faker<Order>()
        .RuleFor(o => o.CustomerId, (faker, order) => string.IsNullOrEmpty(customerId) ? new CustomerId(faker.Random.AlphaNumeric(7)) : new CustomerId(customerId))
        .RuleFor(o => o.EmployeeId, (faker, order) => new EmployeeId(faker.Random.Number(1, 20)))
        .RuleFor(o => o.OrderDate, (faker, order) => DateTime.Now)
        .RuleFor(o => o.RequiredDate, (faker, order) => DateTime.Now)
        .RuleFor(o => o.ShippedDate, (faker, order) => DateTime.Now)
        .RuleFor(o => o.ShipVia, (faker, order) => faker.Random.Number(1, 10))
        .RuleFor(o => o.Freight, (faker, order) => 10m)
        .RuleFor(o => o.ShipName, (faker, order) => faker.Company.CompanySuffix())
        .RuleFor(o => o.ShippingAddress, (faker, t) =>
                    Addresses.Create(
                        faker.Address.FullAddress(),
                        faker.Address.City(),
                        faker.Address.State(),
                        faker.Address.ZipCode(),
                        faker.Address.Country()))
        .RuleFor(o => o.Employee, (faker, order) => new Employee(new EmployeeId(faker.Random.Number(1, 20)),
            faker.Name.JobTitle(), faker.Name.FirstName(), faker.Name.LastName(), faker.Name.JobTitle(),
            faker.Person.DateOfBirth, DateTime.Now, faker.Phone.PhoneNumber(), "ext", faker.Random.Bytes(90),
            "Notes", 1, "TestPath", Addresses.Create(
                        faker.Address.FullAddress(),
                        faker.Address.City(),
                        faker.Address.State(),
                        faker.Address.ZipCode(),
                        faker.Address.Country())))
        .RuleFor(o => o.OrderDetails, (faker, order) => GetOrderDetails(order.Id.Value).Generate(7));

    private static Faker<OrderDetail> GetOrderDetails(int orderId) => new Faker<OrderDetail>()
        .RuleFor(od => od.Id, (faker, orderDetail) => new OrderDetailId(faker.Random.Number(10, 99)))
        .RuleFor(od => od.OrderId, (faker, orderDetail) => new OrderId(orderId))
        .RuleFor(od => od.ProductId, (faker, orderDetail) => faker.Random.Number(10, 99))
        .RuleFor(od => od.UnitPrice, (faker, orderDetail) => faker.Random.Decimal(10, 99))
        .RuleFor(od => od.Quantity, (faker, orderDetail) => faker.Random.Number(1, 10));

    public static Faker<Product> GetProducts() => new Faker<Product>()
        .RuleFor(pro => pro.ProductId, (faker, product) => faker.Random.Number(10, 99))
        .RuleFor(pro => pro.ProductName, (faker, product) => faker.Commerce.ProductName())
        .RuleFor(pro => pro.SupplierId, (faker, product) => faker.Random.Number(10, 99))
        .RuleFor(pro => pro.CategoryId, (faker, product) => faker.Random.Number(10, 99))
        .RuleFor(pro => pro.QuantityPerUnit, (faker, product) => "1")
        .RuleFor(pro => pro.UnitPrice, (faker, product) => faker.Random.Decimal(10, 99))
        .RuleFor(pro => pro.UnitsInStock, (faker, product) => faker.Random.Number(10, 99))
        .RuleFor(pro => pro.UnitsOnOrder, (faker, product) => faker.Random.Number(10, 99))
        .RuleFor(pro => pro.ReorderLevel, (faker, product) => faker.Random.Number(10, 99))
        .RuleFor(pro => pro.Discontinued, (faker, product) => faker.Random.Bool());
}
