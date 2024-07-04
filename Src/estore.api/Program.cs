namespace estore.api;

using System.Net;
using System.Reflection;
using estore.api.Common;
using estore.api.Extensions;
using estore.api.Middleware;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.common.Common.Results;
using estore.common.Models.Responses;
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
            [FromServices] IEmployeeRepository repository) =>
        {
            var response = await repository.FindByConditionAsync(x => x.Id == new EmployeeId(employeeId));

            return response.Any() ?
                Results.Ok(Result<EmployeeResponse>.SuccessResult(response.Select(x => new EmployeeResponse
                {
                    Title = x.Title,
                    FullName = $"{x.FirstName} {x.LastName}",
                    EmployeeId = employeeId
                }).Single())) :
                Results.NotFound(Result<EmployeeResponse>.FailedResult($"Employee not found with Id:{employeeId}", HttpStatusCode.NotFound));
        })
         .WithTags("Employee")
         .Produces<Result<EmployeeResponse>>((int)HttpStatusCode.OK)
         .Produces<Result<EmployeeResponse>>((int)HttpStatusCode.NotFound);

        app.Run();
    }
}
