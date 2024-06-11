namespace orchestrator.service.Persistance.Entities;

using MassTransit;
using System;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }

    public string? CustomerId { get; set; } = string.Empty;

    public int? OrderId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? FailedOn { get; set; }

    public string? ErrorMessage { get; set; }

    public string JsonOrderRequest { get; set; } = string.Empty;
}
