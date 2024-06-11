namespace orchestrator.service.Extensions;

using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using orchestrator.service.Configurations;
using orchestrator.service.Consumers;
using orchestrator.service.Persistance.Context;
using orchestrator.service.Persistance.Entities;
using orchestrator.service.WorkFlows;

internal static class EventBusExtension
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(EventBusSetting)).Get<EventBusSetting>()!;

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<ServiceStateMachine, OrderState>()
                    .EntityFrameworkRepository(ef =>
                    {
                        ef.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                        ef.AddDbContext<DbContext, StateDbContext>((provider, builder) =>
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                sqlOptions.MigrationsHistoryTable($"__{nameof(StateDbContext)}");
                            }));
                    });

            cfg.AddConsumersFromNamespaceContaining<CreateCustomerConsumer>();

            cfg.AddBusConfigurator(settings);
        });

        return services;
    }
}
