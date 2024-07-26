namespace orchestrator.api.Consumers;

using System.Threading.Tasks;
using estore.common.Events;
using MassTransit;
using orchestrator.api.Services;

public class DeleteOrderWithCustomerConsumer(IEStoreService eStoreService) : IConsumer<DeleteOrderWithCustomerEvent>
{
    private readonly IEStoreService _eStoreService = eStoreService;

    public async Task Consume(ConsumeContext<DeleteOrderWithCustomerEvent> context)
    {
        if (context.Message.OrderId > 0)
        {
            var result = await _eStoreService.DeleteOrder(context.Message.OrderId);

            if (!result.IsSuccess)
            {
                await context.RespondAsync(new FailedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    ConsumerName = nameof(CreateCustomerConsumer),
                    ErrorMessage = result.ErrorMessages!.First().ToString()
                });
            }
        }

        if (!string.IsNullOrEmpty(context.Message.CustomerId))
        {
            var result = await _eStoreService.DeleteCustomer(context.Message.CustomerId);

            if (!result.IsSuccess)
            {
                await context.RespondAsync(new FailedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    ConsumerName = nameof(CreateCustomerConsumer),
                    ErrorMessage = result.ErrorMessages!.First().ToString()
                });
            }
        }
        return;
    }
}
