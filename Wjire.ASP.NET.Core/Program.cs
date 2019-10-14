namespace Wjire.ASP.NET.Core
{
    public class Program
    {
        public static void Main()
        {
            new WebHostBuilder()
                .UseHttpListener()
                .Configure(app => app
                    .Use(FooMiddleware)
                    .Use(BarMiddleware)
                    .Use(BazMiddleware))
                .Build()  
                .StartAsync();
        }

        public static RequestDelegate FooMiddleware(RequestDelegate next)
        {
            return async context =>
                        {
                            await context.Response.WriteAsync("Foo=>");
                            await next(context);
                        };
        }

        public static RequestDelegate BarMiddleware(RequestDelegate next)
        {
            return async context =>
                        {
                            await context.Response.WriteAsync("Bar=>");

                            await next(context);
                        };
        }

        public static RequestDelegate BazMiddleware(RequestDelegate next)
        {
            return context => context.Response.WriteAsync("Baz");
        }
    }
}
