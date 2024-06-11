namespace estore.common.Models.Requests;

using estore.common.Common.Pagination;

public class SearchOrderRequest : QueryStringParameters
{
    public string? CustomerId { get; set; } = string.Empty;

    public int? EmployeeId { get; set; }
}
