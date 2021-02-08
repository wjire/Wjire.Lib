using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace Wjire.Redis.RedLock.Demo
{
    public class Startup
    {
        private RedLockFactory _redLockFactory;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var endPoints = new List<RedLockEndPoint>
            {
                new DnsEndPoint("127.0.0.1", 6379),
                new DnsEndPoint("127.0.0.1", 6380),
                new DnsEndPoint("127.0.0.1", 6381)
            };
            _redLockFactory = RedLockFactory.Create(endPoints);
            services.AddSingleton(typeof(IDistributedLockFactory), _redLockFactory);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //应用程序结束时释放,因为不是容器创建的对象
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                _redLockFactory.Dispose();
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
