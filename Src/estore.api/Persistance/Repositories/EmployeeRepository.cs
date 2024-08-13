namespace estore.api.Persistance.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using estore.api.Models.Aggregates.Employee;
using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository(EStoreDBContext context) : IEmployeeRepository
{
    private readonly EStoreDBContext _context = context;

    public void Add(Employee entity) => _context.Add(entity);

    public async Task<IEnumerable<Employee>> FindByConditionAsync(Expression<Func<Employee, bool>> expression) =>
       await GetAll().Where(expression).ToListAsync();

    public IQueryable<Employee> GetAll() =>
        _context.Employees
        .Include(x => x.EmployeeTerritories)
        .AsNoTracking();

    public void Update(Employee entity) => _context.Update(entity);
}
