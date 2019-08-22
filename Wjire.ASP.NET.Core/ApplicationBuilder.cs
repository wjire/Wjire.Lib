using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wjire.ASP.NET.Core
{
    public class ApplicationBuilder : IApplicationBuilder
    {

        private readonly List<Func<RequestDelegate, RequestDelegate>> _middlewares =
            new List<Func<RequestDelegate, RequestDelegate>>();

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }

        public RequestDelegate Build()
        {
            _middlewares.Reverse();
            return httpContext =>
            {
                RequestDelegate next = context => { context.Response.StatusCode = 404; return Task.CompletedTask; };
                foreach (Func<RequestDelegate, RequestDelegate> middleware in _middlewares)
                {
                    next = middleware(next);
                }
                return next(httpContext);
            };
        }
    }
}
