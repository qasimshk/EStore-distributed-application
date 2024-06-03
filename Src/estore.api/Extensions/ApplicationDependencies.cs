namespace estore.api.Extensions;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Orders;
using estore.api.Persistance.Context;
using estore.api.Persistance.Repositories;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EStoreDBContext>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
