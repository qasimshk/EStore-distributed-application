namespace estore.api.Models.Requests;

using estore.api.Common.Pagination;

public class SearchOrderRequest : QueryStringParameters
{
    public string? CustomerId { get; set; } = string.Empty;

    public int? EmployeeId { get; set; }
}
