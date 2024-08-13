namespace estore.api.Persistance.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Aggregates.Orders.Entities;
using estore.api.Models.Aggregates.Orders.ValueObjects;
using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;

public class OrderRepository(EStoreDBContext context) : IOrderRepository
{
    private readonly EStoreDBContext _context = context;

    public void Add(Order entity) => _context.Orders.Add(entity);

    public async Task<IEnumerable<Order>> FindByConditionAsync(Expression<Func<Order, bool>> expression) =>
        await GetAll().Where(expression).ToListAsync();

    public IQueryable<Order> GetAll() => _context.Orders
        .Include(ord => ord.Customer)
        .Include(ord => ord.Employee)
        .Include(ord => ord.OrderDetails)
        .ThenInclude(orderDetail => orderDetail.Item)
        .ThenInclude(item => item.Category)
        .AsNoTracking();

    public void Update(Order entity) => _context.Orders.Update(entity);

    public void AddOrderDetails(List<OrderDetail> orderDetails) => _context.AddRange(orderDetails);
    public async Task<IEnumerable<Product>> Products(Expression<Func<Product, bool>> expression) =>
        await _context.Products
        .Include(pro => pro.Category)
        .Where(expression)
        .ToListAsync();

    public async Task DeleteOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(ord => ord.OrderDetails)
            .Where(ord => ord.Id == new OrderId(orderId))
            .SingleAsync();

        _context.OrderDetails.RemoveRange(order.OrderDetails);

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
    }
}
