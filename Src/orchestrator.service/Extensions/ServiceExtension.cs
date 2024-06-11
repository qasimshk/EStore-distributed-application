namespace orchestrator.service.Extensions;

using System.Net.Http;
using System.Net.Http.Headers;
using orchestrator.service.Services;

public static class ServiceExtension
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
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

        return services;
    }
}
