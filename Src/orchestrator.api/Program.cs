using System.Net;
using System.Reflection;
using estore.common.Common.Results;
using estore.common.Events;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using orchestrator.api.Extensions;
using orchestrator.api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
        .AddEnvironmentVariables()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services
    .AddServiceConfiguration(builder.Configuration)
    .AddEventBus(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.MapGet("/api/payment/{correlationId}:Guid/state", async (
   [FromRoute] Guid correlationId,
   [FromServices] IEStoreService service) => await service.GetPaymentState(correlationId))
   .WithName("PaymentState")
   .WithTags("Payment")
   .Produces<PaymentStateEvent>((int)HttpStatusCode.OK)
   .Produces<PaymentInformationEvent>((int)HttpStatusCode.NotFound);

app.MapGet("/api/order-submitted/{correlationId}:Guid/state", async (
   [FromRoute] Guid correlationId,
   [FromServices] IEStoreService service) => await service.GetOrderState(correlationId))
   .WithName("OrderState")
   .WithTags("Order")
   .Produces<OrderStateEvent>((int)HttpStatusCode.OK)
   .Produces<OrderInformationEvent>((int)HttpStatusCode.NotFound);

app.MapGet("/api/order-submitted/{correlationId}:Guid/refund", async (
   [FromRoute] Guid correlationId,
   [FromServices] IEStoreService service) => await service.RefundOrder(correlationId))
   .WithName("RefundOrder")
   .WithTags("Order")
   .Produces<OrderStateEvent>((int)HttpStatusCode.OK)
   .Produces<OrderInformationEvent>((int)HttpStatusCode.NotFound);

app.MapDelete("/api/order-submitted/{correlationId}:Guid/remove", async (
   [FromRoute] Guid correlationId,
   [FromServices] IEStoreService service) => await service.RemoveOrder(correlationId))
   .WithName("RemoveOrder")
   .WithTags("Order")
   .Produces((int)HttpStatusCode.Accepted)
   .Produces<OrderInformationEvent>((int)HttpStatusCode.NotFound);

app.MapPost("/api/order", async (
   [FromBody] SubmitOrderRequest request,
   [FromServices] IEStoreService service) => await service.SubmitOrder(request))
   .WithName("SubmitOrder")
   .WithTags("Order")
   .Produces<Result<OrderResponse>>((int)HttpStatusCode.OK)
   .Produces<Result>((int)HttpStatusCode.BadRequest);

app.Run();
