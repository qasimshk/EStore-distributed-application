namespace gateway.api.Extensions;

using MassTransit;

public static class CustomConfigurationExtensions
{
    public static void ApplyCustomMassTransitConfiguration(this IBusRegistrationConfigurator configurator) =>
        configurator.SetEndpointNameFormatter(new CustomEndpointNameFormatter());
}
