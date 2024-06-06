namespace estore.api.Models.Aggregates.Orders;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.api.Models.Aggregates.Orders.Entities;
using estore.api.Models.Aggregates.Orders.ValueObjects;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Customer.ValueObjects;

public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderDetail> _orderDetails = [];

    public CustomerId CustomerId { get; }

    public EmployeeId EmployeeId { get; }

    public DateTime OrderDate { get; }

    public DateTime RequiredDate { get; }

    public DateTime? ShippedDate { get; }

    public int ShipVia { get; }

    public decimal Freight { get; }

    public string ShipName { get; }

    public Addresses ShippingAddress { get; }

    public Customer Customer { get; }

    public Employee Employee { get; }

    public IReadOnlyList<OrderDetail> OrderDetails => _orderDetails.AsReadOnly();

    private Order() { }

    private Order(OrderId orderId,
        CustomerId customerId,
        EmployeeId employeeId,
        DateTime orderDate,
        DateTime requiredDate,
        DateTime shippingDate,
        int shipVia,
        decimal freight,
        string shipName,
        Addresses shippingAddress) : base(orderId)
    {
        CustomerId = customerId;
        EmployeeId = employeeId;
        OrderDate = orderDate;
        RequiredDate = requiredDate;
        ShippedDate = shippingDate;
        ShipVia = shipVia;
        Freight = freight;
        ShipName = shipName;
        ShippingAddress = shippingAddress;
    }

    public static Order Create(Customer customer,
        Employee employee,
        DateTime orderDate,
        DateTime requiredDate,
        DateTime shippingDate,
        int shipVia,
        decimal freight,
        string shipName,
        Addresses shippingAddress) => new(OrderId.CreateUnique(),
            customer.Id,
            employee.Id,
            orderDate,
            requiredDate,
            shippingDate,
            shipVia,
            freight,
            shipName,
            shippingAddress);
}
