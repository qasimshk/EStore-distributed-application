namespace estore.api.Models.Aggregates.Employee.ValueObjects;

using estore.api.Common.Models;
using estore.api.Extensions;

public class EmployeeId : ValueObject
{
    public int Value { get; }

    protected EmployeeId() { }

    private EmployeeId(int value) => Value = value;

    public static EmployeeId CreateUnique() => new(Guid.NewGuid().GuidToInteger());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
