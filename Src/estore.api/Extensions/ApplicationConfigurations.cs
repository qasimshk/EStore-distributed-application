namespace estore.api.Extensions;
public static class ApplicationConfigurations
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}
