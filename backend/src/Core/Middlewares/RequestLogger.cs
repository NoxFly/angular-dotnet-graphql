namespace Core.Middlewares;

public class RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.Accept.ToString().Contains("application/json"))
        {
            await _next(context);
            return;
        }

        var request = context.Request;
        var response = context.Response;

        var requestTime = DateTime.Now;

        await _next(context);

        var responseTime = DateTime.Now;

        var duration = responseTime - requestTime;

        var durationFormatted = string.Format("{0}{1}{2}",
            duration.Minutes > 0 ? $"{duration.Minutes}m" : "",
            duration.Seconds > 0 ? $"{duration.Seconds}s" : "",
            duration.Milliseconds > 0 ? $"{duration.Milliseconds}ms" : "");

        _logger.LogTrace($"[{requestTime}] CLIENT > {response.StatusCode} {durationFormatted} - {request.Method} {request.Path}");
    }
}
