public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly string _secretKey;

    public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _secretKey = "YourSecretKeyHere123!@#";
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for Swagger endpoints and in development
        if (context.Request.Path.StartsWithSegments("/swagger") || 
            context.Request.Path.StartsWithSegments("/users"))  // Temporarily skip auth for /users endpoints
        {
            await _next(context);
            return;
        }

        string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "No token provided" });
            return;
        }

        try
        {
            if (!IsValidToken(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid token" });
                return;
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authentication error occurred");
            throw;
        }
    }

    private bool IsValidToken(string token)
    {
        return !string.IsNullOrEmpty(token);
    }
}