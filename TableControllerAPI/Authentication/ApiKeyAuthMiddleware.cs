using System.Diagnostics;
using DotNetEnv;

namespace TableControllerApi.Authentication;
public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string apiKey;
    public ApiKeyAuthMiddleware(RequestDelegate next)
    {
        _next = next;
        string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envPath);
        apiKey = Env.GetString("TCAPI_KEY");
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key missing");
            return;
        }
        if (apiKey == null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync($"Api Key not set \"{apiKey}\"");
            return;
        }

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid Api Key");
            return;
        }

        await _next(context);
    }
}
        