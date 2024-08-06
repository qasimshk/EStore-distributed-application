namespace estore.api.Extensions;

using estore.api.Abstractions.Mappers;
using estore.api.Abstractions.Services;
using estore.api.Mappers;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Orders;
using estore.api.Persistance.Repositories;
using estore.api.Services;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<ICustomerServices, CustomerServices>();
        services.AddScoped<IOrderServices, OrderServices>();
        services.AddScoped<IGeneralServices, GeneralServices>();

        // Mappers
        services.AddScoped<ICustomerMapper, CustomerMapper>();
        services.AddScoped<IOrderMapper, OrderMapper>();

        return services;
    }
}
