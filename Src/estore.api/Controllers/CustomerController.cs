namespace estore.api.Controllers;

using System.Net;
using System.Text.Json;
using estore.api.Abstractions.Services;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[Controller]")]
public class CustomerController(ICustomerServices customerServices) : Controller
{
    private readonly ICustomerServices _customerServices = customerServices;

    [HttpGet("{customerId}", Name = nameof(GetCustomerByCustomerId))]
    [ProducesResponseType(typeof(Result<CustomerResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCustomerByCustomerId([FromRoute] string customerId)
    {
        var result = await _customerServices.GetCustomerByCustomerId(customerId);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("phone/{phoneNumber}", Name = nameof(GetCustomerByPhoneNumber))]
    [ProducesResponseType(typeof(Result<CustomerResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCustomerByPhoneNumber([FromRoute] string phoneNumber)
    {
        var result = await _customerServices.GetCustomerByPhoneNumber(phoneNumber);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("search", Name = nameof(GetCustomerBySearchParameters))]
    [ProducesResponseType(typeof(PagedList<CustomerResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCustomerBySearchParameters([FromQuery] SearchCustomerRequest search)
    {
        var result = await _customerServices.GetCustomers(search);

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

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest model)
    {
        var result = await _customerServices.CreateCustomer(model);

        return result.IsSuccess ?
            CreatedAtRoute(nameof(GetCustomerByCustomerId), new { customerId = result.Value }, result) :
            BadRequest(result);
    }

    [HttpDelete("delete/{customerId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] string customerId)
    {
        var result = await _customerServices.DeleteCustomer(customerId);

        return result.IsSuccess ? NoContent() : NotFound(result);
    }
}
