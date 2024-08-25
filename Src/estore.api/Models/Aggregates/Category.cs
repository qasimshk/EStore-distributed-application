namespace estore.api.Models.Aggregates;

using estore.api.Extensions;

public sealed class Category
{
    private readonly List<Product> _products = [];

    public int CategoryId { get; private set; }

    public string CategoryName { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public byte[] Picture { get; private set; }

    public IReadOnlyList<Product> Products => _products.AsReadOnly();

    public static Category Create(string categoryName, string description) => new()
    {
        CategoryId = Guid.NewGuid().GuidToInteger(),
        CategoryName = categoryName,
        Description = description,
        Picture = [0x00, 0xFF, 0xAB, 0xCD, 0x12]
    };
}
