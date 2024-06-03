namespace estore.api.Persistance.Repositories;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using estore.api.Models.Aggregates.Customer;
using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository
{
    private readonly EStoreDBContext _context;

    public CustomerRepository(EStoreDBContext context) => _context = context;

    public void Add(Customer entity) => _context.Add(entity);

    public async Task<IEnumerable<Customer>> FindByConditionAsync(Expression<Func<Customer, bool>> expression) =>
        await _context.Customers.Where(expression).ToListAsync();

    public void Update(Customer entity) => _context.Update(entity);
}
