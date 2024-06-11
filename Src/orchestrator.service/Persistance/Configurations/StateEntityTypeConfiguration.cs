namespace orchestrator.service.Persistance.Configurations;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchestrator.service.Persistance.Entities;

public class StateEntityTypeConfiguration : SagaClassMap<OrderState>
{
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    {
        entity.ToTable(nameof(OrderState));

        entity.HasKey(x => x.CorrelationId);

        entity.Property(x => x.CurrentState)
            .HasMaxLength(64)
            .IsRequired();

        entity.Property(x => x.CustomerId)
            .IsRequired(false);

        entity.Property(x => x.OrderId)
            .IsRequired(false);

        entity.Property(x => x.EmployeeId)
            .IsRequired(false);

        entity.Property(x => x.CreatedOn)
            .IsRequired();

        entity.Property(x => x.FailedOn)
            .IsRequired(false);

        entity.Property(x => x.ErrorMessage)
            .IsRequired(false);

        entity.Property(x => x.JsonOrderRequest)
            .IsRequired();
    }
}
