namespace estore.api.Services;

using System.Net;
using estore.api.Abstractions.Services;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.api.Persistance.Context;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using Microsoft.EntityFrameworkCore;

public class GeneralServices(EStoreDBContext dbContext,
    IEmployeeRepository employeeRepository,
    IPagedList<ProductResponse> paged) : IGeneralServices
{
    private readonly EStoreDBContext _dbContext = dbContext;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IPagedList<ProductResponse> _paged = paged;

    public async Task<Result<List<CategoryResponse>>> GetCategories()
    {
        var category = await _dbContext.Categories.Select(category => new CategoryResponse
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description!
        }).ToListAsync();

        return Result<List<CategoryResponse>>.SuccessResult(category);
    }

    public async Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId)
    {
        var response = await _employeeRepository.FindByConditionAsync(x => x.Id == new EmployeeId(employeeId));

        return response.Any() ?
            Result<EmployeeResponse>.SuccessResult(response.Select(x => new EmployeeResponse
            {
                EmployeeId = x.Id.Value,
                Title = x.Title,
                FullName = $"{x.FirstName} {x.LastName}",
                Address = x.EmployeeAddress.GetCompleteAddress(),
                Age = x.GetAge(),
                ServiceYears = x.ServiceDuration(),
                ContactNumber = $"{x.HomePhone} EXT:{x.Extension}",
                Notes = x.Notes
            }).Single()) :
            Result<EmployeeResponse>.FailedResult($"Employee not found with Id:{employeeId}", HttpStatusCode.NotFound);
    }

    public PagedList<ProductResponse> GetProducts(SearchProductRequest search)
    {
        var query = _dbContext.Products
            .Include(x => x.Category)
            .AsQueryable();

        if (search.CategoryId > 0)
        {
            query = query.Where(x => x.CategoryId == search.CategoryId);
        }

        if (search.ProductId > 0)
        {
            query = query.Where(x => x.ProductId == search.ProductId);
        }

        var result = _paged.ToPagedList(query.Distinct().Select(pro => new ProductResponse
        {
            ProductName = pro.ProductName,
            CategoryName = pro.Category.CategoryName,
            QuantityPerUnit = pro.QuantityPerUnit,
            ProductId = pro.ProductId,
            CategoryId = pro.CategoryId,
            Discontinued = pro.Discontinued,
            ReorderLevel = pro.ReorderLevel,
            SupplierId = pro.SupplierId,
            UnitPrice = pro.UnitPrice,
            UnitsInStock = pro.UnitsInStock,
            UnitsOnOrder = pro.UnitsOnOrder,
        }).AsQueryable(), search.PageNumber, search.PageSize);

        return result;
    }
}
