using Bookify.Api.Middleware;

namespace Bookify.Api.Extensions;

public static class MiddlewareExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseRootRouteOk(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RootRouteMiddleware>();
    }

}
