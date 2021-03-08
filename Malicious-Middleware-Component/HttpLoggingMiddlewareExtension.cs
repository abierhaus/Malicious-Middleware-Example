using Microsoft.AspNetCore.Builder;

namespace Malicious_Middleware_Component
{
    public static class HttpLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseMaliciousHttpLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpLoggingMiddleware>();
        }
    }
}