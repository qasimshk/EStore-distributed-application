namespace estore.api.Models.Aggregates;

using System.Collections.Generic;
using estore.api.Common.Models;

public class Addresses : ValueObject
{
    public string? Address { get; }

    public string? City { get; }

    public string? Region { get; }

    public string? PostalCode { get; }

    public string? Country { get; }

    private Addresses(string address, string city, string region, string postalCode, string country)
    {
        Address = address;
        City = city;
        Region = region;
        Country = country;
        PostalCode = postalCode;
    }

    public static Addresses Create(string address,
        string city,
        string region,
        string postalCode,
        string country) => new(address, city, region, postalCode, country);

    public string GetCompleteAddress() =>
        string.Join(", ", new string[] { Address, City, Region, PostalCode, Country }
              .Where(address => !string.IsNullOrEmpty(address)));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
        yield return City;
        yield return Region;
        yield return PostalCode;
        yield return Country;
    }
}
