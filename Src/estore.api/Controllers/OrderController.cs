namespace estore.api.Controllers;

using System.Net;
using estore.api.Abstractions.Services;
using estore.api.Common.Results;
using estore.api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[Controller]")]
public class OrderController(IOrderServices orderServices) : Controller
{
    private readonly IOrderServices _orderServices = orderServices;

    [HttpGet("{orderId}", Name = nameof(GetOrderByOrderId))]
    [ProducesResponseType(typeof(Result<OrderResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOrderByOrderId([FromRoute] int orderId)
    {
        var result = await _orderServices.GetOrderByOrderId(orderId);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
