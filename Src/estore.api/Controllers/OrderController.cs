namespace estore.api.Controllers;

using System.Net;
using System.Text.Json;
using estore.api.Abstractions.Services;
using estore.api.Common.Pagination;
using estore.api.Common.Results;
using estore.api.Models.Requests;
using estore.api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[Controller]")]
public class OrderController(IOrderServices orderServices) : Controller
{
    private readonly IOrderServices _orderServices = orderServices;

    [HttpGet("{orderId}", Name = nameof(GetOrderByOrderId))]
    [ProducesResponseType(typeof(Result<CustomerOrderResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOrderByOrderId([FromRoute] int orderId)
    {
        var result = await _orderServices.GetOrderByOrderId(orderId);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedList<OrderResponse>), (int)HttpStatusCode.OK)]
    public IActionResult GetOrderBySearch([FromQuery] SearchOrderRequest search)
    {
        var response = _orderServices.GetOrderBySearch(search);

        var metadata = new
        {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.TotalPages,
            response.HasNext,
            response.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Result<OrderResponse>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest model)
    {
        var result = await _orderServices.CreateOrder(model);

        return result.IsSuccess ?
            CreatedAtRoute(nameof(GetOrderByOrderId), new { orderId = result.Value.OrderId }, result) :
            BadRequest(result);
    }
}
