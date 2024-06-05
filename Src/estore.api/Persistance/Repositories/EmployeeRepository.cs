namespace estore.api.Persistance.Repositories;

using System;
using System.Collections.Generic;
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
       await _context.Employees.Where(expression).ToListAsync();

    public void Update(Employee entity) => _context.Update(entity);
}
