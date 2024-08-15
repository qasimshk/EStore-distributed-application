namespace orchestrator.api.Consumers;

using System.Net;
using estore.common.Events;
using estore.common.Models.Requests;
using MassTransit;
using orchestrator.api.Services;

public class CreateCustomerConsumer(IEStoreService eStoreService) : IConsumer<CreateCustomerEvent>
{
    private readonly IEStoreService _eStoreService = eStoreService;

    public async Task Consume(ConsumeContext<CreateCustomerEvent> context)
    {
        var customer = await _eStoreService.GetCustomerByPhoneNumber(context.Message.Phone);

        if (customer.StatusCode == HttpStatusCode.OK)
        {
            await context.RespondAsync(new CustomerCreatedSuccessfullyEvent
            {
                CorrelationId = context.Message.CorrelationId,
                CustomerId = customer.Value.CustomerId
            });
        }
        else
        {
            var result = await _eStoreService.CreateCustomer(CreateCustomerRequest.Map(context.Message));

            if (result.IsSuccess)
            {
                await context.RespondAsync(new CustomerCreatedSuccessfullyEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    CustomerId = result.Value.CustomerID
                });
            }
            else
            {
                await context.RespondAsync(new FailedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    ConsumerName = nameof(CreateCustomerConsumer),
                    ErrorMessage = result.ErrorMessage
                });
            }
        }
        return;
    }
}
