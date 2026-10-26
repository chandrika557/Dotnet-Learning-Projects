using System.Net;
using System.Text.Json;
using LibraryApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (status, title) = ex switch //pattern matching is used to determine the appropriate HTTP status code and title based on the type of exception. If the exception is a NotFoundException, it returns a 404 Not Found status with the title "Resource not found". For any other exception, it defaults to a 500 Internal Server Error status with the title "An unexpected error occurred".
        {
            NotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        if (status == HttpStatusCode.InternalServerError)
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
        else
            _logger.LogWarning("{Title}: {Message}", title, ex.Message);

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            // Show the real message only for expected errors, never for 500s
            Detail = status == HttpStatusCode.InternalServerError ? "Please try again later." : ex.Message,
            Instance = context.Request.Path
        };
        problem.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}