using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Exceptions;

namespace TaskManagementSystem.WebApi.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public  GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteProblemDetailsAsync(context, ex);
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
    {
        var (statusCode, title) = MapException(ex);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = ex.Message,
            Type = $"https://httpstatuses.com/{statusCode}",
            Instance = context.Request.Path
        };
        
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;
        
        return context.Response.WriteAsJsonAsync(problem);
    }

    private static (int statusCode, string title) MapException(Exception ex)
    {
        return ex switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation error"),
            NotFoundException => (StatusCodes.Status404NotFound, "Not found"),
            ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),

            ArgumentException => (StatusCodes.Status400BadRequest, "Validation error"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid operation"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),

            _ => ((int)HttpStatusCode.InternalServerError, "Server error")
        };
    }
}