namespace orchestrator.api.Extensions;

using MassTransit;

public static class CustomExtensions
{
    public static void ApplyCustomMassTransitConfiguration(this IBusRegistrationConfigurator configurator) =>
        configurator.SetEndpointNameFormatter(new CustomEndpointNameFormatter());
}
