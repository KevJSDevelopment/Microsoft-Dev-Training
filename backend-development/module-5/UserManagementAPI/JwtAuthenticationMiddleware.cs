public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly string _secretKey; // In production, use proper configuration

    public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _secretKey = "YourSecretKeyHere123!@#"; // In production, use configuration
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "No token provided" });
            return;
        }

        try
        {
            // Validate JWT token here
            // In a real application, use proper JWT validation
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
        // Implement proper JWT validation here
        // This is just a placeholder
        return !string.IsNullOrEmpty(token);
    }
}