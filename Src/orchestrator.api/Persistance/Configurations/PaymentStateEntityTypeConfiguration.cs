namespace orchestrator.api.Persistance.Configurations;

using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using orchestrator.api.Persistance.Entities;

public class PaymentStateEntityTypeConfiguration : SagaClassMap<PaymentState>
{
    protected override void Configure(EntityTypeBuilder<PaymentState> entity, ModelBuilder model)
    {
        entity.ToTable(nameof(PaymentState));

        entity.HasKey(x => x.CorrelationId);

        entity.Property(x => x.CurrentState)
            .HasMaxLength(64)
            .IsRequired();

        entity.Property(x => x.OrderId)
            .IsRequired();

        entity.Property(x => x.Amount)
            .HasColumnType("money")
            .IsRequired();

        entity.Property(x => x.CreatedOn)
            .IsRequired();

        entity.Property(x => x.FailedOn)
            .IsRequired(false);

        entity.Property(x => x.ErrorMessage)
            .IsRequired(false);
    }
}
