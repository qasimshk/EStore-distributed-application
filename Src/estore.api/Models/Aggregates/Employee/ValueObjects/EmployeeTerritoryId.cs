namespace estore.api.Models.Aggregates.Employee.ValueObjects;

using estore.api.Common;
using estore.api.Extensions;

public sealed class EmployeeTerritoryId : ValueObject
{
    public int Value { get; }

    protected EmployeeTerritoryId() { }

    public EmployeeTerritoryId(int value) => Value = value;

    public static EmployeeTerritoryId CreateUnique() => new(Guid.NewGuid().GuidToInteger());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
