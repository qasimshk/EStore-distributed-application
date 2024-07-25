namespace orchestrator.api.Consumers;

using estore.common.Events;
using estore.common.Models.Requests;
using MassTransit;
using orchestrator.api.Services;

public class OrderCreatedConsumer(IEStoreService eStoreService) : IConsumer<CreateOrderEvent>
{
    private readonly IEStoreService _eStoreService = eStoreService;

    public async Task Consume(ConsumeContext<CreateOrderEvent> context)
    {
        var response = await _eStoreService.CreateOrder(CreateOrderRequest.Map(context.Message));

        if (response.IsSuccess)
        {
            await context.RespondAsync(new OrderCreatedSuccessfullyEvent
            {
                CorrelationId = context.Message.CorrelationId,
                OrderId = response.Value.OrderId,
            });
        }
        else
        {
            await context.RespondAsync(new FailedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                ConsumerName = nameof(OrderCreatedConsumer),
                ErrorMessage = response.ErrorMessage
            });
        }
        return;
    }
}
