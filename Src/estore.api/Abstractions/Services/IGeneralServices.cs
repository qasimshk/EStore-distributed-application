namespace estore.api.Abstractions.Services;

using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;

public interface IGeneralServices
{
    Task<Result<List<CategoryResponse>>> GetCategories();

    Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId);

    Task<PagedList<ProductResponse>> GetProducts(SearchProductRequest search);
}
