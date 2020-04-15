using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Wjire.ASP.NET.Core3._1.Demo.Extensions
{
    public static class EnableRequestBufferingMiddleware
    {
        public static void EnableRequestBuffering(this IApplicationBuilder application)
        {
            application.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next.Invoke();
            });
        }
    }
}
