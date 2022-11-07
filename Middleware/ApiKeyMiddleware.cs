using System.Net;

namespace Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    public ApiKeyMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        string apiKeyHeader = "X-API-KEY";
        if (!context.Request.Headers.TryGetValue(apiKeyHeader, out var extractedApiKey))
        {
            context.Response.StatusCode = ((int)HttpStatusCode.Unauthorized);
            await context.Response.WriteAsync($"{apiKeyHeader} header was not provided.");
            return;
        }

        var apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new NullReferenceException("API_KEY");

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = ((int)HttpStatusCode.Unauthorized);
            await context.Response.WriteAsync("Unauthorized.");
            return;
        }

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}
  
public static class ApiKeyMiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="Middleware.ApiKeyMiddleware"/> to the specified <see cref="Microsoft.AspNetCore.Builder.IApplicationBuilder"/>, which authenticates each endpoint by requiring an X-API-KEY header. The inbound key is compared with an "API_KEY" environment variable for equality.
    /// </summary>
    public static IApplicationBuilder UseApiKeyAuthentication(
        this IApplicationBuilder builder) => builder.UseMiddleware<ApiKeyMiddleware>();
}