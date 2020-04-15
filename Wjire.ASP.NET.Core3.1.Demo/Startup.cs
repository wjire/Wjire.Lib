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
                    //ȫ���쳣������
                    configure.Filters.Add<GlobalExceptionFilter>();

                    //ģ����֤������
                    configure.Filters.Add<ModelValidateFilter>();
                })
                //Nuget:Microsoft.AspNetCore.Mvc.NewtonsoftJson
                .AddNewtonsoftJson();

            //��ֹ�Զ�ģ����֤,�����false,��ôģ����֤δͨ����,���Զ�����400,������������,�޷��Զ��巵��ֵ
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            //�����첽��ȡ����body�ͷ���body
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            #region ���ݿ�

            services.AddDapper(x =>
            {
                x.UseSqlServer(configure =>
                {
                    configure.Read = _configuration.GetSection("ConnectionStrings")["Read"];
                    configure.Write = _configuration.GetSection("ConnectionStrings")["Write"];
                });
            });

            #endregion

            #region ����

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

            #region �ϴ�����

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

            // ע����֤Ҫ��Ĵ���������ͨ�����ַ�ʽ��ͬһ��Ҫ����Ӷ�����֤
            services.AddScoped<IAuthorizationHandler, ValidJtiHandler>();
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new ValidJtiRequirement()) // ����������֤Ҫ��
                    .Build());
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidAudience = _configuration[JwtConst.JwtAudience],//Audience
                        ValidIssuer = _configuration[JwtConst.JwtIssuer],//Issuer���������ǰ��ǩ��jwt������һ��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[JwtConst.JwtSecurityKey])),//�õ�SecurityKey
                        ClockSkew = TimeSpan.Zero
                    };
                });


            #endregion

            #region Redis

            //Nuget:CSRedisCore
            RedisHelper.Initialization(new CSRedis.CSRedisClient(_configuration["Redis"]));

            #endregion

            #region ����Ƶ������ 

            //Nuget:AspNetCoreRateLimit
            services.AddOptions();
            // �洢IP�����������ù���
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // �����ĵ�����������3.x���breaking change��Ҫ����
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

            #region �Զ������

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
