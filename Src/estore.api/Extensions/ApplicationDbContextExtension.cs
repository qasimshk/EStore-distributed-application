namespace estore.api.Extensions;

using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;

public static class ApplicationDbContextExtension
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EStoreDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null)));

        return services;
    }
}
