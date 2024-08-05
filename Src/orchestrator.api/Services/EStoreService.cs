namespace orchestrator.api.Services;

using estore.common.Common.Results;
using estore.common.Events;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text;
using System.Text.Json;

public class EStoreService(HttpClient httpClient,
    IValidator<SubmitOrderRequest> validator,
    IRequestClient<OrderStateRequestEvent> orderStateRequest,
    IRequestClient<PaymentStateRequestEvent> paymentStateRequest,
    IRequestClient<SubmitOrderEvent> submitOrderRequest,
    IRequestClient<RefundOrderEvent> refundOrder,
    IRequestClient<RemoveOrderEvent> removeOrder) : IEStoreService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IValidator<SubmitOrderRequest> _validator = validator;
    private readonly IRequestClient<OrderStateRequestEvent> _orderStateRequest = orderStateRequest;
    private readonly IRequestClient<PaymentStateRequestEvent> _paymentStateRequest = paymentStateRequest;
    private readonly IRequestClient<SubmitOrderEvent> _submitOrderRequest = submitOrderRequest;
    private readonly IRequestClient<RefundOrderEvent> _refundOrder = refundOrder;
    private readonly IRequestClient<RemoveOrderEvent> _removeOrder = removeOrder;

    public async Task<Result<CreateCustomerResponse>> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = JsonSerializer.Serialize(request);

        var requestContent = new StringContent(customer, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("Customer", requestContent);

        return (await response.Content.ReadFromJsonAsync<Result<CreateCustomerResponse>>())!;
    }

    public async Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request)
    {
        var order = JsonSerializer.Serialize(request);

        var requestContent = new StringContent(order, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("Order", requestContent);

        var content = await response.Content.ReadAsStringAsync();

        return (await response.Content.ReadFromJsonAsync<Result<OrderResponse>>())!;
    }

    public async Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId)
    {
        var response = await _httpClient.GetAsync($"Employee/{employeeId}");

        return (await response.Content.ReadFromJsonAsync<Result<EmployeeResponse>>())!;
    }

    public async Task<Result> DeleteCustomer(string customerId)
    {
        var response = await _httpClient.DeleteAsync($"customer/delete/{customerId}");

        return (await response.Content.ReadFromJsonAsync<Result>())!;
    }

    public async Task<Result> DeleteOrder(int orderId)
    {
        var response = await _httpClient.DeleteAsync($"order/delete/{orderId}");

        return (await response.Content.ReadFromJsonAsync<Result>())!;
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

    public async Task<IResult> GetPaymentState(Guid correlationId)
    {
        var response = await _paymentStateRequest.GetResponse<PaymentStateEvent, PaymentInformationEvent>(new PaymentStateRequestEvent
        {
            CorrelationId = correlationId
        });

        if (response.Is(result: out Response<PaymentStateEvent> paymentFound))
        {
            return Results.Ok(new
            {
                paymentFound.Message.CorrelationId,
                paymentFound.Message.CreatedOn,
                paymentFound.Message.CurrentState,
                paymentFound.Message.OrderId,
                Amount = $"£{paymentFound.Message.Amount}",
                paymentFound.Message.ErrorMessage,
                paymentFound.Message.FailedOn,
            });
        }
        else if (response.Is(result: out Response<PaymentInformationEvent> paymentNotFound))
        {
            return Results.NotFound(new OrderInformationEvent
            {
                CorrelationId = paymentNotFound.Message.CorrelationId,
                Message = paymentNotFound.Message.Message,
            });
        }
        return Results.BadRequest();
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
}
