namespace gateway.api.Extensions;

using estore.common.Common.Pagination;
using FluentValidation;
using gateway.api.Services;
using gateway.api.Validator;
using Microsoft.OpenApi.Models;
using Polly.Extensions.Http;
using Polly;
using System.Net.Http.Headers;

public static class ApplicationDependenciesExtension
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "EStore Online", Version = "v1" }));

        services.AddScoped(typeof(IPagedList<>), typeof(PagedList<>));

        services.AddValidatorsFromAssemblyContaining<OrderSubmitValidator>();

        services.AddHttpClient<IEStoreServices, EStoreServices>((serviceProvider, client) =>
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

        // Orchestrator API Health Check
        services.AddHttpClient("OrchestratorAPI", client =>
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions
           .HandleTransientHttpError()
           .OrResult(msg => !msg.IsSuccessStatusCode)
           .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
