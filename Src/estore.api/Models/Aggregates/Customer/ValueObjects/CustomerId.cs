namespace estore.api.Models.Aggregates.Customer.ValueObjects;

using estore.api.Common.Models;
using estore.api.Extensions;

public sealed class CustomerId : ValueObject
{
    public string Value { get; } = string.Empty;

    protected CustomerId() { }

    public CustomerId(string value) => Value = value;

    public static CustomerId CreateUnique() => new(Guid.NewGuid().GuidToString());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
