using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "An exception occurred with message: {Message} on machine {MachineName}. TraceId: {TraceId}",
            exception.Message,
            Environment.MachineName,
            traceId
        );

        var (statusCode, title) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            detail: exception.Message,
            extensions: new Dictionary<string, object?>
            {
                {"traceId",  traceId}
            }
        ).ExecuteAsync(httpContext);

        return true;
    }

    private static (int StatusCode, string Title) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad request"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad request"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Error Server")
        };
    }
}