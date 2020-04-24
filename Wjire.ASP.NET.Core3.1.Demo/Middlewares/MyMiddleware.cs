using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Wjire.ASP.NET.Core3._1.Demo.Models;

namespace Wjire.ASP.NET.Core3._1.Demo.Middlewares
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //person 可以通过自带DI容器依赖注入
        public async Task Invoke(HttpContext httpContext, Person person)
        {
            //do something
            await _next(httpContext);
        }
    }


    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}
