using GarageLog.Application.Exceptions;

namespace GarageLog.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Unauthorized access");

            await WriteError(
                context,
                StatusCodes.Status401Unauthorized,
                "Authentication failed.",
                new[] { ex.Message }
            );
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation failed");

            await WriteError(
                context,
                StatusCodes.Status400BadRequest,
                "Validation failed.",
                ex.Errors
            );
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid argument");

            await WriteError(
                context,
                StatusCodes.Status400BadRequest,
                "Invalid request.",
                new[] { ex.Message }
            );
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation");

            await WriteError(
                context,
                StatusCodes.Status400BadRequest,
                "Operation failed.",
                new[] { ex.Message }
            );
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");

            await WriteError(
                context,
                StatusCodes.Status404NotFound,
                "Resource not found.",
                new[] { ex.Message }
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            await WriteError(
                context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                new[] { "An unexpected error occurred." }
            );
        }
    }

    private static async Task WriteError(
        HttpContext context,
        int statusCode,
        string message,
        IEnumerable<string> errors
    )
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new { message, errors });
    }
}
