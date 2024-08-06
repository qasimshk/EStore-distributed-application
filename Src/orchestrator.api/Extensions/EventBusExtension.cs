namespace orchestrator.api.Extensions;

using System.Reflection;
using estore.common.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using orchestrator.api.Configurations;
using orchestrator.api.Consumers;
using orchestrator.api.Persistance.Context;
using orchestrator.api.Persistance.Entities;
using orchestrator.api.WorkFlows;

internal static class EventBusExtension
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StateDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null)));

        var settings = configuration.GetSection(nameof(EventBusSetting)).Get<EventBusSetting>()!;

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(ef => ef.AddEntityFrameworkConfiguration(configuration));

            cfg.AddSagaStateMachine<PaymentStateMachine, PaymentState>()
                .EntityFrameworkRepository(ef => ef.AddEntityFrameworkConfiguration(configuration));

            cfg.AddConsumersFromNamespaceContaining<CreateCustomerConsumer>();

            cfg.AddConfigureEndpointsCallback((name, cfg) =>
                cfg.UseMessageRetry(r => r.Immediate(2)));

            cfg.AddBusConfigurator(settings);

            cfg.AddRequestClient<SubmitOrderEvent>();
            cfg.AddRequestClient<RefundOrderEvent>();
            cfg.AddRequestClient<RemoveOrderEvent>();
            cfg.AddRequestClient<OrderStateRequestEvent>();
            cfg.AddRequestClient<PaymentStateRequestEvent>();
        });

        return services;
    }

    private static IEntityFrameworkSagaRepositoryConfigurator AddEntityFrameworkConfiguration(this IEntityFrameworkSagaRepositoryConfigurator config, IConfiguration configuration)
    {
        config.ConcurrencyMode = ConcurrencyMode.Optimistic;

        config.AddDbContext<DbContext, StateDbContext>((provider, builder) =>
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                sqlOptions.MigrationsHistoryTable($"__{nameof(StateDbContext)}");
            }));

        return config;
    }
}
