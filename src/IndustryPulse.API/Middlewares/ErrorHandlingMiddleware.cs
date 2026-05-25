using System.Net;
using System.Text.Json;

namespace IndustryPulse.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
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
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(
                context, ex, HttpStatusCode.NotFound);
        }
        catch (InvalidOperationException ex)
        {
            await HandleExceptionAsync(
                context, ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Message}", ex.Message);
            await HandleExceptionAsync(
            context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception ex,
        HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message = ex.Message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}