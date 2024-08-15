namespace estore.api.Models.Aggregates.Employee.Entities;

using estore.api.Common;
using estore.api.Models.Aggregates.Employee.ValueObjects;

public sealed class EmployeeTerritory : Entity<EmployeeTerritoryId>
{
    public EmployeeId EmployeeId { get; }

    public string TerritoryId { get; }

    public Employee Employee { get; }

    public Territory Territory { get; set; }

    private EmployeeTerritory() { }
}
