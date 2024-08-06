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
        app.Run();
    }
}
