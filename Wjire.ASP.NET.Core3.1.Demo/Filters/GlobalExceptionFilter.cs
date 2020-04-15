using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wjire.ASP.NET.Core3._1.Demo.Extensions;
using Wjire.ASP.NET.Core3._1.Demo.JWT;
using Wjire.Common;
using Wjire.Log;

namespace Wjire.ASP.NET.Core3._1.Demo
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ICapPublisher _capBus;

        public GlobalExceptionFilter(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        public async void OnException(ExceptionContext context)
        {
            LoginUser loginUser = context.HttpContext.GetLoginUser();

            ExceptionLog exceptionLog = new ExceptionLog
            {
                Message = context.Exception.Message,
                Exception = context.Exception.ToString(),
                IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                RequestInfo = await context.HttpContext.GetRequestBody(),
                RouteInfo = context.GetControllerAndAction(),
                Account = loginUser == null ? "未知" : loginUser.Account
            };

            _capBus.PublishAsync("ExceptionLog", exceptionLog);

            if (context.Exception is CustomException)
            {
                context.Result = new OkObjectResult(new
                {
                    Code = 400,
                    Msg = context.Exception.Message
                });
                return;
            }

            LogService.WriteExceptionAsync(context.Exception, exceptionLog.RouteInfo, exceptionLog.RequestInfo);
            context.Result = new OkObjectResult(new
            {
                Code = 400,
                Msg = "当前操作失败,请稍后重试!"
            });
        }
    }
}
