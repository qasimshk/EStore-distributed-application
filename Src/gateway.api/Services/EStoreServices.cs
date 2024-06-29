namespace gateway.api.Services;

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Events;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.WebUtilities;

public class EStoreServices(HttpClient httpClient,
    IValidator<SubmitOrderRequest> validator,
    IRequestClient<OrderStateRequestEvent> orderStateRequest,
    IRequestClient<SubmitOrderEvent> submitOrderRequest,
    IRequestClient<RefundOrderEvent> refundOrder,
    IRequestClient<RemoveOrderEvent> removeOrder) : IEStoreServices
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IValidator<SubmitOrderRequest> _validator = validator;
    private readonly IRequestClient<OrderStateRequestEvent> _orderStateRequest = orderStateRequest;
    private readonly IRequestClient<SubmitOrderEvent> _submitOrderRequest = submitOrderRequest;
    private readonly IRequestClient<RefundOrderEvent> _refundOrder = refundOrder;
    private readonly IRequestClient<RemoveOrderEvent> _removeOrder = removeOrder;

    public async Task<IResult> GetCustomerById(string customerId)
    {
        var response = await _httpClient.GetAsync($"Customer/{customerId}");

        var result = await response.Content.ReadFromJsonAsync<Result<CustomerResponse>>();

        return result!.IsSuccess
            ? Results.Ok(result)
            : Results.NotFound();
    }

    public async Task<IResult> GetCustomerBySearch(SearchCustomerRequest search, HttpContext http)
    {
        var query = new Dictionary<string, string?>
        {
            ["ContactName"] = search.ContactName ?? string.Empty,
            ["CompanyName"] = search.CompanyName ?? string.Empty,
            ["ContactTitle"] = search.ContactTitle ?? string.Empty,
            ["CustomerId"] = search.CustomerId ?? string.Empty,
            ["PageNumber"] = string.Format(CultureInfo.CurrentCulture, "{0}", search.PageNumber),
            ["PageSize"] = string.Format(CultureInfo.CurrentCulture, "{0}", search.PageSize)
        };

        var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("Customer/search", query));

        var result = await response.Content.ReadFromJsonAsync<PagedList<CustomerResponse>>();

        if (result!.Any())
        {
            http.Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(GetMetaData(response.Headers)));

            return Results.Ok(result);
        }
        return Results.NotFound();
    }

    public async Task<IResult> GetOrderByOrderId(int orderId)
    {
        var response = await _httpClient.GetAsync($"order/{orderId}");

        var result = await response.Content.ReadFromJsonAsync<Result<OrderResponse>>();

        return result!.IsSuccess
            ? Results.Ok(result)
            : Results.NotFound(result);
    }

    public async Task<IResult> GetOrderBySearch(SearchOrderRequest search, HttpContext http)
    {
        var query = new Dictionary<string, string?>
        {
            ["CustomerId"] = search.CustomerId ?? string.Empty,
            ["EmployeeId"] = string.Format(CultureInfo.CurrentCulture, "{0}", search.EmployeeId),
            ["PageNumber"] = string.Format(CultureInfo.CurrentCulture, "{0}", search.PageNumber),
            ["PageSize"] = string.Format(CultureInfo.CurrentCulture, "{0}", search.PageSize)
        };

        var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("Order/search", query));

        var result = await response.Content.ReadFromJsonAsync<PagedList<OrderResponse>>();

        if (result!.Any())
        {
            http.Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(GetMetaData(response.Headers)));

            return Results.Ok(result);
        }
        return Results.NotFound();
    }

    public async Task<IResult> SubmitOrder(SubmitOrderRequest submit)
    {
        var result = await _validator.ValidateAsync(submit);

        if (result.IsValid)
        {
            var request = SubmitOrderEvent.Map(submit);

            var response = await _submitOrderRequest.GetResponse<OrderSubmittedEvent>(request);

            return Results.Ok(response);
        }
        return Results.BadRequest(Result<OrderSubmittedResponse>
            .FailedResult(result.Errors.Select(x => x.ErrorMessage).First(), HttpStatusCode.BadRequest));
    }

    public async Task<IResult> RefundOrder(Guid correlationId)
    {
        var response = await _refundOrder.GetResponse<OrderStateEvent, OrderInformationEvent>(new RefundOrderEvent
        {
            CorrelationId = correlationId
        });

        if (response.Is(result: out Response<OrderStateEvent> orderRefunded))
        {
            return Results.Ok(new OrderStateEvent
            {
                CorrelationId = orderRefunded.Message.CorrelationId,
                CreatedOn = orderRefunded.Message.CreatedOn,
                CurrentState = orderRefunded.Message.CurrentState,
                CustomerId = orderRefunded.Message.CustomerId,
                EmployeeId = orderRefunded.Message.EmployeeId,
                ErrorMessage = orderRefunded.Message.ErrorMessage,
                FailedOn = orderRefunded.Message.FailedOn,
                OrderId = orderRefunded.Message.OrderId,
            });
        }
        else if (response.Is(result: out Response<OrderInformationEvent> orderNotFound))
        {
            return Results.NotFound(new OrderInformationEvent
            {
                CorrelationId = orderNotFound.Message.CorrelationId,
                Message = orderNotFound.Message.Message,
            });
        }
        return Results.BadRequest();
    }

    public async Task<IResult> RemoveOrder(Guid correlationId)
    {
        var response = await _removeOrder.GetResponse<OrderInformationEvent>(new RemoveOrderEvent
        {
            CorrelationId = correlationId
        });

        if (!string.IsNullOrEmpty(response.Message.Message))
        {
            return Results.NotFound(new OrderInformationEvent
            {
                CorrelationId = response.Message.CorrelationId,
                Message = response.Message.Message,
            });
        }
        return Results.Accepted();
    }

    public async Task<IResult> GetOrderState(Guid correlationId)
    {
        var response = await _orderStateRequest.GetResponse<OrderStateEvent, OrderInformationEvent>(new OrderStateRequestEvent
        {
            CorrelationId = correlationId
        });

        if (response.Is(result: out Response<OrderStateEvent> orderFound))
        {
            return Results.Ok(new OrderStateEvent
            {
                CorrelationId = orderFound.Message.CorrelationId,
                CreatedOn = orderFound.Message.CreatedOn,
                CurrentState = orderFound.Message.CurrentState,
                CustomerId = orderFound.Message.CustomerId,
                EmployeeId = orderFound.Message.EmployeeId,
                ErrorMessage = orderFound.Message.ErrorMessage,
                FailedOn = orderFound.Message.FailedOn,
                OrderId = orderFound.Message.OrderId,
            });
        }
        else if (response.Is(result: out Response<OrderInformationEvent> orderNotFound))
        {
            return Results.NotFound(new OrderInformationEvent
            {
                CorrelationId = orderNotFound.Message.CorrelationId,
                Message = orderNotFound.Message.Message,
            });
        }
        return Results.BadRequest();
    }

    private static object GetMetaData(HttpResponseHeaders header)
    {
        IEnumerable<string> values;

        if (header.TryGetValues("X-Pagination", values: out values!))
        {
            var list = values.First().Split(',');

            var metadata = new
            {
                TotalCount = int.Parse(GetMetaDataValue(list[0]), CultureInfo.CurrentCulture),
                PageSize = int.Parse(GetMetaDataValue(list[1]), CultureInfo.CurrentCulture),
                CurrentPage = int.Parse(GetMetaDataValue(list[2]), CultureInfo.CurrentCulture),
                TotalPages = int.Parse(GetMetaDataValue(list[3]), CultureInfo.CurrentCulture),
                HasNext = bool.Parse(GetMetaDataValue(list[4])),
                HasPrevious = bool.Parse(GetMetaDataValue(list[5]))
            };
            return metadata;
        }
        return string.Empty;
    }

    private static string GetMetaDataValue(string value) => value.Split(':').Last().Replace("}", "");
}
