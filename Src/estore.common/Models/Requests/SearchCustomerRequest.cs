namespace estore.common.Models.Requests;

using estore.common.Common.Pagination;

public class SearchCustomerRequest : QueryStringParameters
{
    public string? CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }

    public string? CustomerId { get; set; }
}
