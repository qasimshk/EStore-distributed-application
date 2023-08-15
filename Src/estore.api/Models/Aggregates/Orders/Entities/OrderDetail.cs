namespace estore.api.Models.Aggregates.Orders.Entities;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Orders.ValueObjects;

public class OrderDetail : Entity<OrderDetailId>
{
    public OrderId OrderId { get; }

    public int ProductId { get; }

    public decimal UnitPrice { get; }

    public int Quantity { get; }

    public double Discount { get; }

    public Order Orders { get; }

    private OrderDetail() { }

    private OrderDetail(OrderDetailId orderDetailId,
        Order orders,
        int productId,
        decimal unitPrice,
        int quantity,
        double discount) : base(orderDetailId)
    {
        OrderId = orders.Id;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
    }

    public static OrderDetail Create(Order order,
        int productId,
        decimal unitPrice,
        int quantity,
        double discount) => new(OrderDetailId.CreateUnique(), order, productId, unitPrice, quantity, discount);
}
