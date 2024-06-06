namespace estore.api.Models.Requests;

using estore.api.Common.Pagination;

public class SearchCustomer : QueryStringParameters
{
    public string? CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }

    public string? CustomerId { get; set; }
}
