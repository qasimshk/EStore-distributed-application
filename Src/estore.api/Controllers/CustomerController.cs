namespace estore.api.Controllers;

using System.Net;
using estore.api.Abstractions.Services;
using estore.api.Common.Results;
using estore.api.Models.Responses;
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
}
