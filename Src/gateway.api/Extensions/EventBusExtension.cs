namespace gateway.api.Extensions;

using estore.common.Events;
using gateway.api.Configurations;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class EventBusExtension
{
    public static IServiceCollection AddEventBusService(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(EventBusSetting)).Get<EventBusSetting>()!;

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(config =>
        {
            config.ApplyCustomMassTransitConfiguration();

            config.SetKebabCaseEndpointNameFormatter();

            config.AddBusConfigurator(settings);

            config.AddRequestClient<SubmitOrderEvent>();

            config.AddRequestClient<RefundOrderEvent>();

            config.AddRequestClient<RemoveOrderEvent>();

            config.AddRequestClient<OrderStateRequestEvent>();

            config.AddRequestClient<PaymentStateRequestEvent>();
        });

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        return services;
    }
}
