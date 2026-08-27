using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace proxy_simulator.Services;
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            HttpRequestException => StatusCodes.Status502BadGateway,
            TaskCanceledException => StatusCodes.Status408RequestTimeout,
            JsonException => StatusCodes.Status500InternalServerError,
            InvalidOperationException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        _logger.LogError(
            exception,
            "Unhandled exception. Path: {Path}",
            httpContext.Request.Path);

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message = exception.Message
            },
            cancellationToken);

        return true;
    }
}