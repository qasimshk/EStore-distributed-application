namespace orchestrator.service.Consumers;

using System.Threading.Tasks;
using estore.common.Events;
using MassTransit;
using orchestrator.service.Extensions;

public class NotificationConsumer : IConsumer<SendCustomerNotificationEvent>
{
    public async Task Consume(ConsumeContext<SendCustomerNotificationEvent> context)
    {
        try
        {
            await Task.Delay(1000);

            StaticMethod.PrintMessage("Notification send to the customer");

            Task.CompletedTask.Wait();
        }
        catch (Exception ex)
        {
            await context.RespondAsync(new FailedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                ConsumerName = nameof(CreateCustomerConsumer),
                ErrorMessage = ex.ToString()
            });
        }
    }
}
