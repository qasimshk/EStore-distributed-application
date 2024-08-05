namespace orchestrator.api.Extensions;

using System.Net.Http.Headers;
using FluentValidation;
using Microsoft.OpenApi.Models;
using orchestrator.api.Services;
using orchestrator.api.Validators;

public static class ServiceExtension
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<OrderSubmitValidator>();

        services.AddHttpClient<IEStoreService, EStoreService>((serviceProvider, client) =>
        {
            var serviceUrl = configuration.GetSection("ServiceEndpoint").Value!;

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.BaseAddress = new Uri(serviceUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Orchestrator API",
            Version = "v1"
        }));

        services.AddHealthChecks();

        return services;
    }
}
