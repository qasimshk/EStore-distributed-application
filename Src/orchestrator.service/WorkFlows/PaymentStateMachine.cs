namespace orchestrator.service.WorkFlows;

using estore.common.Events;
using MassTransit;
using orchestrator.service.Extensions;
using orchestrator.service.Persistance.Entities;

public class PaymentStateMachine : MassTransitStateMachine<PaymentState>
{
    // States
    public State Failed { get; set; }
    public State Successful { get; set; }
    public State Refunded { get; set; }

    // Events
    public Event<OrderCreatedSuccessfullyEvent> OrderCreatedSuccessfullyEvents { get; set; }
    public Event<RefundOrderEvent> RefundOrderEvents { get; set; }
    public Event<RemoveOrderEvent> RemoveOrderEvents { get; set; }
    public Event<FailedEvent> FailedEvents { get; set; }
    public Event<PaymentStateRequestEvent> PaymentStateRequestEvents { get; set; }

    public PaymentStateMachine()
    {
        InstanceState(s => s.CurrentState);

        Event(() => OrderCreatedSuccessfullyEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => RefundOrderEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => RemoveOrderEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => FailedEvents, order => order.CorrelateById(x => x.Message.CorrelationId));

        Event(() => PaymentStateRequestEvents, order =>
        {
            order.CorrelateById(order => order.Message.CorrelationId);
            order.OnMissingInstance(order => order.ExecuteAsync(async context =>
            {
                if (context.RequestId.HasValue)
                {
                    await context.RespondAsync<PaymentInformationEvent>(new
                    {
                        context.Message.CorrelationId,
                        Message = "Payment not found"
                    });
                }
            }));
        });

        Initially(
            When(OrderCreatedSuccessfullyEvents)
            .Then(context =>
            {
                context.Saga.OrderId = context.Message.OrderId;
                context.Saga.CreatedOn = DateTime.UtcNow;
                context.Saga.CorrelationId = context.Message.CorrelationId;
                context.Saga.Amount = GetOrderAmount();
                StaticMethod.PrintMessage("Payment received successfully!");
            })
            .TransitionTo(Successful));

        DuringAny(
            When(RefundOrderEvents)
            .Then(context =>
            {
                context.Saga.Amount = 0.00m;
                StaticMethod.PrintMessage("Payment refunded successfully!");
            })
            .TransitionTo(Refunded),

            When(FailedEvents)
            .Then(context =>
            {
                context.Saga.FailedOn = DateTime.Now;
                context.Saga.ErrorMessage = context.Message.ErrorMessage;
                StaticMethod.PrintMessage("Payment process failed");
            })
            .TransitionTo(Failed),

            When(PaymentStateRequestEvents)
            .RespondAsync(x => x.Init<PaymentStateEvent>(new PaymentStateEvent
            {
                CorrelationId = x.Saga.CorrelationId,
                CurrentState = x.Saga.CurrentState,
                OrderId = x.Saga.OrderId,
                Amount = x.Saga.Amount,
                CreatedOn = x.Saga.CreatedOn,
                ErrorMessage = x.Saga.ErrorMessage,
                FailedOn = x.Saga.FailedOn
            })),

            When(RemoveOrderEvents)
            .Finalize());

        SetCompletedWhenFinalized();
    }

    private static decimal GetOrderAmount()
    {
        var rand = new Random();
        var r = rand.Next(0, 10000);
        return ((10000 - r) * 10 + r * 1000) / 10000;
    }
}
