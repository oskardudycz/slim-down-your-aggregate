using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace PublishingHouse.Api.Core;

public static class ExceptionHandlingMiddleware
{
    public static IApplicationBuilder UseExceptionHandlerWithDefaultMapping(
        this IApplicationBuilder app,
        Func<Exception, HttpContext, (string Title, string Detail, int StatusCode)>? customExceptionMap = null
    ) =>
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                if (context.RequestServices.GetService<IProblemDetailsService>() is not { } problemDetailsService)
                    return;

                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exceptionType = exceptionHandlerFeature?.Error;

                if (exceptionType is null)
                    return;

                var details = (customExceptionMap ?? MapException)(exceptionType, context);

                var problem = new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails = { Title = details.Title, Detail = details.Detail, Status = details.StatusCode }
                };

                problem.ProblemDetails.Extensions.Add("exception", exceptionHandlerFeature?.Error.ToString());

                await problemDetailsService.WriteAsync(problem);
            });
        });

    private static (string Title, string Detail, int StatusCode) MapException(
        Exception exceptionType,
        HttpContext context
    )
    {
        (string Title, string Detail, int StatusCode) details = exceptionType switch
        {
            UnauthorizedAccessException =>
            (
                exceptionType.GetType().Name,
                exceptionType.Message,
                context.Response.StatusCode = StatusCodes.Status401Unauthorized
            ),
            NotImplementedException =>
            (
                exceptionType.GetType().Name,
                exceptionType.Message,
                context.Response.StatusCode = StatusCodes.Status501NotImplemented
            ),
            ArgumentException =>
            (
                exceptionType.GetType().Name,
                exceptionType.Message,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            InvalidOperationException =>
            (
                exceptionType.GetType().Name,
                exceptionType.Message,
                context.Response.StatusCode = StatusCodes.Status403Forbidden
            ),
            ValidationException =>
            (
                exceptionType.GetType().Name,
                exceptionType.Message,
                context.Response.StatusCode = StatusCodes.Status403Forbidden
            ),
            _ =>
            (
                exceptionType.GetType().Name,
                exceptionType.Message,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };
        return details;
    }
}
