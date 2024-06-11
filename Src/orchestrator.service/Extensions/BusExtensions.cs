namespace orchestrator.service.Extensions;

using System;
using MassTransit;
using orchestrator.service.Configurations;

public static class BusExtensions
{
    public static IBusRegistrationConfigurator AddBusConfigurator(this IBusRegistrationConfigurator configurator, EventBusSetting eventBusSetting)
    {
        configurator.ApplyCustomMassTransitConfiguration();

        configurator.SetKebabCaseEndpointNameFormatter();

        configurator.UsingRabbitMq((context, rabbitMqBusFactoryConfigurator) =>
        {
            // if cloud amqp account is used then make sure to add user name as well after host name in config for example cloudamqp.example.com/jetzodxo:jetzodxo

            if (eventBusSetting.EventBusConnection != string.Empty)
            {
                rabbitMqBusFactoryConfigurator.Host(eventBusSetting.EventBusConnection);
            }
            else
            {
                rabbitMqBusFactoryConfigurator.Host(new Uri($"rabbitmq://{eventBusSetting.EventBusHost}/"), hostConfigurator =>
                {
                    hostConfigurator.Username(eventBusSetting.EventBusUser);
                    hostConfigurator.Password(eventBusSetting.EventBusPassword);
                });
            }

            MessageDataDefaults.ExtraTimeToLive = TimeSpan.FromDays(1);
            MessageDataDefaults.Threshold = 2000;
            MessageDataDefaults.AlwaysWriteToRepository = false;

            rabbitMqBusFactoryConfigurator.ConfigureEndpoints(context);
        });

        return configurator;
    }
}
