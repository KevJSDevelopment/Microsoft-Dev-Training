public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Log the incoming request
            _logger.LogInformation(
                "Request {Method} {Path} started at {Time}",
                context.Request.Method,
                context.Request.Path,
                DateTime.UtcNow);

            await _next(context);

            // Log the response
            _logger.LogInformation(
                "Request {Method} {Path} completed with status {StatusCode} at {Time}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Request {Method} {Path} failed at {Time}",
                context.Request.Method,
                context.Request.Path,
                DateTime.UtcNow);
            throw;
        }
    }
}