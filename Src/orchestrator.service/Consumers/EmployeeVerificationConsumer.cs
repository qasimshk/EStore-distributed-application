namespace orchestrator.service.Consumers;

using System.Threading.Tasks;
using estore.common.Events;
using MassTransit;
using orchestrator.service.Services;

public class EmployeeVerificationConsumer(IEStoreService eStoreService) : IConsumer<VerifyEmployeeEvent>
{
    private readonly IEStoreService _eStoreService = eStoreService;

    public async Task Consume(ConsumeContext<VerifyEmployeeEvent> context)
    {
        var response = await _eStoreService.GetEmployeeById(context.Message.EmployeeId);

        if (response.IsSuccess)
        {
            await context.RespondAsync(new EmployeeVerifiedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                EmployeeId = context.Message.EmployeeId,
                Verified = response.IsSuccess
            });
        }
        else
        {
            await context.RespondAsync(new FailedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                ConsumerName = nameof(EmployeeVerificationConsumer),
                ErrorMessage = response.ErrorMessage
            });
        }
        return;
    }
}
