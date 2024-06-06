namespace estore.api.Models.Aggregates;

using estore.api.Models.Aggregates.Employee.Entities;

public sealed class Territory
{
    public string TerritoryId { get; } = string.Empty;

    public string TerritoryDescription { get; } = string.Empty;

    public int RegionId { get; }

    public Region Region { get; }

    public EmployeeTerritory? EmployeeTerritory { get; }
}
