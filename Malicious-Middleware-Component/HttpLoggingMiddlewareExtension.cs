using Microsoft.AspNetCore.Builder;

namespace Malicious_Middleware_Component
{
    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class HttpLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseMaliciousHttpLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpLoggingMiddleware>();
        }
    }
}