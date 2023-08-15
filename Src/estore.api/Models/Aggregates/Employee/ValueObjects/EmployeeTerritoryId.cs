namespace estore.api.Models.Aggregates.Employee.ValueObjects;

using estore.api.Common.Models;
using estore.api.Extensions;

public class EmployeeTerritoryId : ValueObject
{
    public int Value { get; }

    protected EmployeeTerritoryId() { }

    private EmployeeTerritoryId(int value) => Value = value;

    public static EmployeeTerritoryId CreateUnique() => new(Guid.NewGuid().GuidToInteger());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
