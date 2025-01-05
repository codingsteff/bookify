namespace Bookify.Api.Middleware;

public class RootRouteMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/" && context.Request.Method == HttpMethods.Get)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync("OK");
        }
        else
        {
            await _next(context); // Continue to the next middleware
        }
    }
}