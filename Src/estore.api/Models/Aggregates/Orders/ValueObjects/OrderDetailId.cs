namespace estore.api.Models.Aggregates.Orders.ValueObjects;

using estore.api.Common.Models;
using estore.api.Extensions;

public sealed class OrderDetailId : ValueObject
{
    public int Value { get; }

    protected OrderDetailId() { }

    public OrderDetailId(int value) => Value = value;

    public static OrderDetailId CreateUnique() => new(Guid.NewGuid().GuidToInteger());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
