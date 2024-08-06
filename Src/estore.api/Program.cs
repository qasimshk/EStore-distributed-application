namespace estore.api;

using System.Net;
using System.Reflection;
using estore.api.Abstractions.Services;
using estore.api.Extensions;
using estore.api.Middleware;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        builder.Services
            .AddApplicationConfiguration()
            .AddApplicationDbContext(builder.Configuration)
            .AddApplicationDependencies();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/api/employee/{employeeId}", async (
            [FromRoute] int employeeId,
            [FromServices] IGeneralServices service) => await service.GetEmployeeById(employeeId))
         .WithTags("General")
         .Produces<Result<EmployeeResponse>>((int)HttpStatusCode.OK)
         .Produces<Result<EmployeeResponse>>((int)HttpStatusCode.NotFound);

        app.MapGet("/api/categories", async (
            [FromServices] IGeneralServices service) => await service.GetCategories())
         .WithTags("General")
         .Produces<Result<CategoryResponse>>((int)HttpStatusCode.OK);

        app.MapGet("/api/products", (
            [AsParameters] SearchProductRequest search,
            [FromServices] IGeneralServices service,
            HttpContext http) => service.GetProducts(search, http))
         .WithTags("General")
         .Produces<PagedList<ProductResponse>>((int)HttpStatusCode.OK)
         .Produces<PagedList<ProductResponse>>((int)HttpStatusCode.NotFound);

        app.Run();
    }
}
