namespace estore.api.Models.Aggregates;

public class Region
{
    public int RegionId { get; }

    public string RegionDescription { get; } = string.Empty;

    public Territory? Territory { get; }
}
