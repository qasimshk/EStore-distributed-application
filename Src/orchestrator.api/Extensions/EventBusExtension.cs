namespace orchestrator.api.Extensions;

using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using orchestrator.api.Configurations;
using orchestrator.api.Persistance.Entities;
using orchestrator.api.WorkFlows;
using orchestrator.api.Consumers;
using orchestrator.api.Persistance.Context;

internal static class EventBusExtension
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
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
