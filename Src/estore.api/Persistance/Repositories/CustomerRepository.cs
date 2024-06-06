namespace estore.api.Persistance.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using estore.api.Models.Aggregates.Customer;
using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository(EStoreDBContext context) : ICustomerRepository
{
    private readonly EStoreDBContext _context = context;

    public void Add(Customer entity) => _context.Add(entity);

    public async Task<IEnumerable<Customer>> FindByConditionAsync(Expression<Func<Customer, bool>> expression) =>
        await GetAll().Where(expression).ToListAsync();

    public IQueryable<Customer> GetAll() =>
        _context.Customers
        .Include(cus => cus.Orders)
        .ThenInclude(ord => ord.Employee);

    public void Update(Customer entity) => _context.Update(entity);
}
