namespace orchestrator.api.Persistance.Migrations;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using orchestrator.api.Persistance.Context;

[DbContext(typeof(StateDbContext))]
partial class StateDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.6")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("orchestrator.service.Persistance.Entities.OrderState", b =>
        {
            b.Property<Guid>("CorrelationId")
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime>("CreatedOn")
                .HasColumnType("datetime2");

            b.Property<string>("CurrentState")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("CustomerId")
                .HasColumnType("nvarchar(max)");

            b.Property<int?>("EmployeeId")
                .HasColumnType("int");

            b.Property<string>("ErrorMessage")
                .HasColumnType("nvarchar(max)");

            b.Property<DateTime?>("FailedOn")
                .HasColumnType("datetime2");

            b.Property<string>("JsonOrderRequest")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<int?>("OrderId")
                .HasColumnType("int");

            b.HasKey("CorrelationId");

            b.ToTable("OrderState", (string)null);
        });

        modelBuilder.Entity("orchestrator.service.Persistance.Entities.PaymentState", b =>
        {
            b.Property<Guid>("CorrelationId")
                .HasColumnType("uniqueidentifier");

            b.Property<decimal>("Amount")
                .HasColumnType("decimal(18,2)");

            b.Property<DateTime>("CreatedOn")
                .HasColumnType("datetime2");

            b.Property<string>("CurrentState")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("ErrorMessage")
                .HasColumnType("nvarchar(max)");

            b.Property<DateTime?>("FailedOn")
                .HasColumnType("datetime2");

            b.Property<int>("OrderId")
                .HasColumnType("int");

            b.HasKey("CorrelationId");

            b.ToTable("PaymentState", (string)null);
        });
#pragma warning restore 612, 618
    }
}
