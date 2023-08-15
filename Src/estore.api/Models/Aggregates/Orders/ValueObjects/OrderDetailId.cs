namespace estore.api.Models.Aggregates.Orders.ValueObjects;

using estore.api.Common.Models;
using estore.api.Extensions;

public class OrderDetailId : ValueObject
{
    public int Value { get; }

    protected OrderDetailId() { }

    private OrderDetailId(int value) => Value = value;

    public static OrderDetailId CreateUnique() => new(Guid.NewGuid().GuidToInteger());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
