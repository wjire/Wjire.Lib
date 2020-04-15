using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Wjire.ASP.NET.Core3._1.Demo.Extensions;
using Wjire.ASP.NET.Core3._1.Demo.Logics;
using Wjire.ASP.NET.Core3._1.Demo.Utils;
using Wjire.Dapper;
using Wjire.Dapper.SqlServer;

namespace Wjire.ASP.NET.Core3._1.Demo
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(configure =>
                {
                    //全局异常过滤器
                    configure.Filters.Add<GlobalExceptionFilter>();

                    //模型验证过滤器
                    configure.Filters.Add<ModelValidateFilter>();
                })
                //Nuget:Microsoft.AspNetCore.Mvc.NewtonsoftJson
                .AddNewtonsoftJson();

            //禁止自动模型验证,如果是false,那么模型验证未通过后,会自动返回400,结束本次请求,无法自定义返回值
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            //允许异步读取请求body和返回body
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            #region 数据库

            services.AddDapper(x =>
            {
                x.UseSqlServer(configure =>
                {
                    configure.Read = _configuration.GetSection("ConnectionStrings")["Read"];
                    configure.Write = _configuration.GetSection("ConnectionStrings")["Write"];
                });
            });

            #endregion

            #region 跨域

            string[] allowOrigins = _configuration.GetSection("AllowOrigins").Get<string[]>();
            services.AddCors(options =>
                options.AddPolicy("Default",
                    builder => builder.WithOrigins(allowOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        //.AllowAnyOrigin()
                        .AllowCredentials()
                )
            );

            #endregion

            #region 上传设置

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            #endregion

            #region JWT

            services.AddTransient<JwtSecurityTokenHandler>();
            services.AddTransient<JwtTokenUtil>();
            services.AddSingleton<JwtBlackCollection>();
            services.AddSingleton<IHostedService, CleaningJwtBlackCollectionService>();

            // 注册验证要求的处理器，可通过这种方式对同一种要求添加多种验证
            services.AddScoped<IAuthorizationHandler, ValidJtiHandler>();
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new ValidJtiRequirement()) // 添加上面的验证要求
                    .Build());
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = _configuration[JwtConst.JwtAudience],//Audience
                        ValidIssuer = _configuration[JwtConst.JwtIssuer],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[JwtConst.JwtSecurityKey])),//拿到SecurityKey
                        ClockSkew = TimeSpan.Zero
                    };
                });


            #endregion

            #region Redis

            //Nuget:CSRedisCore
            RedisHelper.Initialization(new CSRedis.CSRedisClient(_configuration["Redis"]));

            #endregion

            #region 访问频率限制 

            //Nuget:AspNetCoreRateLimit
            services.AddOptions();
            // 存储IP计数器及配置规则
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // 按照文档，这两个是3.x版的breaking change，要加上
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion

            #region Session

            //TimeSpan sessionTimeout = TimeSpan.FromDays(Convert.ToInt32(_configuration["SessionIdleTimeout"]));
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = sessionTimeout;
            //    options.Cookie.MaxAge = sessionTimeout;
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

            //services.AddDistributedRedisCache(options =>
            //{
            //    IConfigurationSection redisConfig = _configuration.GetSection("Redis");
            //    options.Configuration = redisConfig["Configuration"];
            //    options.InstanceName = redisConfig["InstanceName"];
            //});

            #endregion

            #region 自定义服务

            services.AddSingleton<IHostedService, MyBackgroundService>();
            services.AddSingleton<UploadedFileCheck>();
            services.AddSingleton<OperationLogAttribute>();
            services.AddLogics();

            #endregion
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("Default");
            app.UseIpRateLimiting();
            app.UseAuthentication();
            app.EnableRequestBuffering();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
