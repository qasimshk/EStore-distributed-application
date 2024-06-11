namespace orchestrator.service.WorkFlows;

using System.Text.Json;
using estore.common.Events;
using MassTransit;
using orchestrator.service.Extensions;
using orchestrator.service.Persistance.Entities;

public class ServiceStateMachine : MassTransitStateMachine<OrderState>
{
    // States
    public State OrderSubmitted { get; set; }
    public State CustomerCreated { get; set; }
    public State EmployeeVerified { get; set; }
    public State Completed { get; set; }
    public State OrderFailed { get; set; }
    public State OrderCancelled { get; set; }
    public State OrderRefund { get; set; }

    // Events
    public Event<SubmitOrderEvent> SubmitOrderEvents { get; set; }
    public Event<OrderSubmittedEvent> OrderSubmittedEvents { get; set; }
    public Event<CreateCustomerEvent> CreateCustomerEvents { get; set; }
    public Event<CustomerCreatedSuccessfullyEvent> CustomerCreatedSuccessfullyEvents { get; set; }
    public Event<VerifyEmployeeEvent> VerifyEmployeeEvents { get; set; }
    public Event<EmployeeVerifiedEvent> EmployeeVerifiedEvents { get; set; }
    public Event<OrderStateRequestEvent> OrderStateRequestEvents { get; set; }
    public Event<CreateOrderEvent> CreateOrderEvents { get; set; }
    public Event<OrderCreatedSuccessfullyEvent> OrderCreatedSuccessfullyEvents { get; set; }
    public Event<SendCustomerNotificationEvent> SendCustomerNotificationEvents { get; set; }
    public Event<FailedEvent> FailedEvents { get; set; }
    public Event<RefundOrderEvent> RefundOrderEvents { get; set; }

    public ServiceStateMachine()
    {
        InstanceState(s => s.CurrentState);

        Event(() => SubmitOrderEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => OrderSubmittedEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => CreateCustomerEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => CustomerCreatedSuccessfullyEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => EmployeeVerifiedEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => VerifyEmployeeEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => FailedEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => CreateOrderEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => OrderCreatedSuccessfullyEvents, order => order.CorrelateById(x => x.Message.CorrelationId));
        Event(() => SendCustomerNotificationEvents, order => order.CorrelateById(x => x.Message.CorrelationId));

        Event(() => OrderStateRequestEvents, order =>
        {
            order.CorrelateById(order => order.Message.CorrelationId);
            order.OnMissingInstance(order => order.ExecuteAsync(async context =>
            {
                if (context.RequestId.HasValue)
                {
                    await context.RespondAsync<OrderNotFoundEvent>(new
                    {
                        context.Message.CorrelationId,
                        Message = "Order not found"
                    });
                }
            }));
        });

        Event(() => RefundOrderEvents, order =>
        {
            order.CorrelateById(order => order.Message.CorrelationId);
            order.OnMissingInstance(order => order.ExecuteAsync(async context =>
            {
                if (context.RequestId.HasValue)
                {
                    await context.RespondAsync<OrderNotFoundEvent>(new
                    {
                        context.Message.CorrelationId,
                        Message = "Order not found"
                    });
                }
            }));
        });

        Initially(
            When(SubmitOrderEvents)
            .Then(context =>
            {
                context.Saga.CorrelationId = context.Message.CorrelationId;
                context.Saga.CreatedOn = DateTime.Now;
                context.Saga.EmployeeId = context.Message.EmployeeId;
                context.Saga.JsonOrderRequest = JsonSerializer.Serialize(context.Message);
                StaticMethod.PrintMessage("Order Submitted Successfully");
            })
            .Publish(context => context.Message.CreateCustomer)
            .TransitionTo(OrderSubmitted)
            .RespondAsync(order => order.Init<OrderSubmittedEvent>(new OrderSubmittedEvent
            {
                CorrelationId = order.Saga.CorrelationId,
                OrderState = order.Saga.CurrentState,
                SubmittedDate = order.Saga.CreatedOn.ToShortDateString(),
            })));

        During(OrderSubmitted,
            When(CustomerCreatedSuccessfullyEvents)
            .Then(context =>
            {
                context.Saga.CustomerId = context.Message.CustomerId;
                StaticMethod.PrintMessage("Customer Created Successfully");
            })
            .Publish(context => new VerifyEmployeeEvent
            {
                CorrelationId = context.Message.CorrelationId,
                EmployeeId = context.Saga.EmployeeId!.Value
            })
            .TransitionTo(CustomerCreated));

        During(CustomerCreated,
            When(EmployeeVerifiedEvents)
            .Then(context =>
            {
                context.Saga.EmployeeId = context.Message.EmployeeId;
                StaticMethod.PrintMessage("Employee verified Successfully");
            })
            .Publish(context =>
                    CreateOrderEvent.Map(JsonSerializer.Deserialize<SubmitOrderEvent>(context.Saga.JsonOrderRequest)!,
                    context.Saga.CustomerId!))
            .TransitionTo(EmployeeVerified));

        During(EmployeeVerified,
            When(OrderCreatedSuccessfullyEvents)
            .Then(context =>
            {
                context.Saga.OrderId = context.Message.OrderId;
                StaticMethod.PrintMessage("Order created Successfully");
            })
            .Publish(context => new SendCustomerNotificationEvent
            {
                CustomerId = context.Saga.CustomerId!,
            })
            .TransitionTo(Completed));

        DuringAny(
            When(FailedEvents)
            .Then(context =>
            {
                context.Saga.FailedOn = DateTime.Now;
                context.Saga.ErrorMessage = context.Message.ErrorMessage;
                StaticMethod.PrintMessage("Order creating process failed");
            })
            .TransitionTo(OrderFailed),

            When(OrderStateRequestEvents)
            .RespondAsync(x => x.Init<OrderStateEvent>(new OrderStateEvent
            {
                CorrelationId = x.Saga.CorrelationId,
                CreatedOn = x.Saga.CreatedOn,
                CurrentState = x.Saga.CurrentState,
                CustomerId = x.Saga.CustomerId ?? string.Empty,
                OrderId = x.Saga.OrderId ?? 0,
                EmployeeId = x.Saga.EmployeeId ?? 0,
                ErrorMessage = x.Saga.ErrorMessage,
                FailedOn = x.Saga.FailedOn
            })),

            When(RefundOrderEvents)
            .IfElse(context => context.Saga.CurrentState == nameof(Completed),
            ifActivity => ifActivity.TransitionTo(OrderRefund),
            elseActivity => elseActivity.RespondAsync(x => x.Init<OrderNotFoundEvent>(new OrderNotFoundEvent
            {
                CorrelationId = x.Saga.CorrelationId,
                Message = "Only complete order can be refunded!"
            })))
            .RespondAsync(x => x.Init<OrderStateEvent>(new OrderStateEvent
            {
                CorrelationId = x.Saga.CorrelationId,
                CreatedOn = x.Saga.CreatedOn,
                CurrentState = x.Saga.CurrentState,
                CustomerId = x.Saga.CustomerId ?? string.Empty,
                OrderId = x.Saga.OrderId ?? 0,
                EmployeeId = x.Saga.EmployeeId ?? 0,
                ErrorMessage = x.Saga.ErrorMessage,
                FailedOn = x.Saga.FailedOn
            })));
    }
}
