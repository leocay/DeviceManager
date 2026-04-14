using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(RequestDelegate next, IHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            var validationProblem = new ValidationProblemDetails(
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).Distinct().ToArray()))
            {
                Type = "https://httpstatuses.com/400",
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.Request.Path
            };

            validationProblem.Extensions["errorCode"] = "VALIDATION_ERROR";
            validationProblem.Extensions["traceId"] = context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(validationProblem);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Unexpected Error",
                Detail = _environment.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Instance = context.Request.Path
            };

            problemDetails.Extensions["errorCode"] = "UNHANDLED_ERROR";
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}