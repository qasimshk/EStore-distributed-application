namespace orchestrator.api.Persistance.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class PaymentAmountAdded : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<decimal>(
            name: "Amount",
            table: "PaymentState",
            type: "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "Amount",
            table: "PaymentState");
}
