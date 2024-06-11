namespace estore.api.Models.Aggregates.Employee.ValueObjects;

using estore.api.Common;
using estore.api.Extensions;

public sealed class EmployeeId : ValueObject
{
    public int Value { get; }

    protected EmployeeId() { }

    public EmployeeId(int value) => Value = value;

    public static EmployeeId CreateUnique() => new(Guid.NewGuid().GuidToInteger());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
