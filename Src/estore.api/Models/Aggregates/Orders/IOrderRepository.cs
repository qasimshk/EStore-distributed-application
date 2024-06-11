namespace estore.api.Models.Aggregates.Orders;

using System.Linq.Expressions;
using estore.api.Common;
using estore.api.Models.Aggregates.Orders.Entities;

public interface IOrderRepository : IRepository<Order>
{
    void AddOrderDetails(List<OrderDetail> orderDetails);

    Task<IEnumerable<Product>> Products(Expression<Func<Product, bool>> expression);
}
