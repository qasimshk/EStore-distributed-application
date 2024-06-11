namespace estore.api.Extensions;

using System.Net;
using estore.api.Middleware;
using estore.api.Validations;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
