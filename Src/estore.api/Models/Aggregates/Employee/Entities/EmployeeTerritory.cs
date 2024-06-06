namespace estore.api.Models.Aggregates.Employee.Entities;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Employee.ValueObjects;

public sealed class EmployeeTerritory : Entity<EmployeeTerritoryId>
{
    public EmployeeId EmployeeId { get; }

    public string TerritoryId { get; }

    public Employee Employee { get; }

    public Territory Territory { get; set; }

    private EmployeeTerritory() { }

    private EmployeeTerritory(EmployeeTerritoryId employeeTerritoryId,
        EmployeeId employeeId,
        string territoryId) : base(employeeTerritoryId)
    {
        EmployeeId = employeeId;
        TerritoryId = territoryId;
    }
}
