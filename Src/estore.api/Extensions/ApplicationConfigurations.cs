namespace estore.api.Extensions;

using estore.api.Common.Pagination;
using estore.api.Common.Results;

using System.Net;
using estore.api.Middleware;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using estore.api.Validations;

public static class ApplicationConfigurations
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
        .ConfigureApiBehaviorOptions(opt => opt.InvalidModelStateResponseFactory = context =>
        {
            var errors = new List<string>();

            foreach (var fields in context.ModelState.Keys)
            {
                errors.AddRange(from error in context.ModelState[fields]?.Errors
                                select error.ErrorMessage);
            }

            return new BadRequestObjectResult(Result.FailedResult(errors, HttpStatusCode.BadRequest));
        });

        //Paging
        services.AddScoped(typeof(IPagedList<>), typeof(PagedList<>));

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        // Middlewares
        services.AddTransient<ExceptionHandlingMiddleware>();

        // Fluent Validator
        services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

        return services;
    }
}
