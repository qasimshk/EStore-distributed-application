namespace estore.api.Abstractions.Services;

using estore.common.Common.Pagination;
using estore.common.Models.Requests;
using estore.common.Models.Responses;

public interface IGeneralServices
{
    Task<IResult> GetCategories();

    Task<IResult> GetEmployeeById(int employeeId);

    PagedList<ProductResponse> GetProducts(SearchProductRequest search, HttpContext http);
}
