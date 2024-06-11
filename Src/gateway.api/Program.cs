namespace gateway.api;

using System.Net;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using gateway.api.Extensions;
using gateway.api.Services;
using Microsoft.AspNetCore.Mvc;
using estore.common.Events;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddApplicationDependencies(builder.Configuration)
            .AddEventBusService(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EStore Api"));
        }

        app.MapGet("/customer/{customerId}", async (
            [FromRoute] string customerId,
            [FromServices] IEStoreServices service) => await service.GetCustomerById(customerId))
            .WithName("GetCustomerById")
            .WithTags("Customer")
            .Produces<Result<CustomerResponse>>((int)HttpStatusCode.OK)
            .Produces<Result>((int)HttpStatusCode.NotFound);

        app.MapGet("/customer/search", async (
            [AsParameters] SearchCustomerRequest search,
            [FromServices] IEStoreServices service,
            HttpContext http) => await service.GetCustomerBySearch(search, http))
            .WithName("GetCustomerBySearchParameters")
            .WithTags("Customer")
            .Produces<PagedList<CustomerResponse>>((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.NotFound);

        app.MapGet("/order/{orderId}:int", async (
           [FromRoute] int orderId,
           [FromServices] IEStoreServices service) => await service.GetOrderByOrderId(orderId))
           .WithName("GetOrderByOrderId")
           .WithTags("Order")
           .Produces<Result<OrderResponse>>((int)HttpStatusCode.OK)
           .Produces<Result>((int)HttpStatusCode.NotFound);

        app.MapGet("/order/search", async (
            [AsParameters] SearchOrderRequest search,
            [FromServices] IEStoreServices service,
            HttpContext http) => await service.GetOrderBySearch(search, http))
            .WithName("GetOrderBySearch")
            .WithTags("Order")
            .Produces<PagedList<OrderResponse>>((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.NotFound);

        app.MapGet("/order/{correlationId}:Guid/state", async (
           [FromRoute] Guid correlationId,
           [FromServices] IEStoreServices service) => await service.GetOrderState(correlationId))
           .WithName("GetOrderState")
           .WithTags("Order")
           .Produces<OrderStateEvent>((int)HttpStatusCode.OK)
           .Produces<OrderNotFoundEvent>((int)HttpStatusCode.NotFound);

        app.MapGet("/order/{correlationId}:Guid/refund", async (
           [FromRoute] Guid correlationId,
           [FromServices] IEStoreServices service) => await service.RefundOrder(correlationId))
           .WithName("RefundOrder")
           .WithTags("Order")
           .Produces<OrderStateEvent>((int)HttpStatusCode.OK)
           .Produces<OrderNotFoundEvent>((int)HttpStatusCode.NotFound);

        app.MapPost("/order", async (
           [FromBody] SubmitOrderRequest request,
           [FromServices] IEStoreServices service) => await service.SubmitOrder(request))
           .WithName("SubmitOrder")
           .WithTags("Order")
           .Produces<Result<OrderResponse>>((int)HttpStatusCode.OK)
           .Produces<Result>((int)HttpStatusCode.BadRequest);

        app.Run();
    }
}
