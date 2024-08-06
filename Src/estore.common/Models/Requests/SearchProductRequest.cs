namespace estore.common.Models.Requests;

using estore.common.Common.Pagination;

public class SearchProductRequest : QueryStringParameters
{
    public int? CategoryId { get; set; } = 0;

    public int? ProductId { get; set; } = 0;
}
