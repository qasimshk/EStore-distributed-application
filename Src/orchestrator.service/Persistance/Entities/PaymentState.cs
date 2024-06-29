namespace orchestrator.service.Persistance.Entities;

using System;
using MassTransit;

public class PaymentState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? FailedOn { get; set; }
    public string? ErrorMessage { get; set; }
}
