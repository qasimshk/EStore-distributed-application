namespace estore.api.Controllers;

using System.Net;
using System.Text.Json;
using estore.api.Abstractions.Services;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class GeneralController(IGeneralServices services) : Controller
{
    private readonly IGeneralServices _services = services;

    [HttpGet("api/employee/{employeeId}")]
    [ProducesResponseType(typeof(Result<EmployeeResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetEmployeeById(int employeeId) => Ok(await _services.GetEmployeeById(employeeId));

    [HttpGet("/api/categories")]
    [ProducesResponseType(typeof(Result<CategoryResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategories() => Ok(await _services.GetCategories());

    [HttpGet("/api/products")]
    [ProducesResponseType(typeof(Result<CategoryResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts([FromQuery] SearchProductRequest search)
    {
        var result = await _services.GetProducts(search);

        var metadata = new
        {
            result.TotalCount,
            result.PageSize,
            result.CurrentPage,
            result.TotalPages,
            result.HasNext,
            result.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

        return Ok(result);
    }

}
