namespace estore.api.Models.Aggregates;

public sealed class Category
{
    private readonly List<Product> _products = [];

    public int CategoryId { get; }

    public string CategoryName { get; } = string.Empty;

    public string? Description { get; }

    public byte[] Picture { get; }

    public IReadOnlyList<Product> Products => _products.AsReadOnly();
}
