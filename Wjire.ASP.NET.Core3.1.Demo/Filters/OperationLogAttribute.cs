using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wjire.ASP.NET.Core3._1.Demo.Extensions;
using Wjire.ASP.NET.Core3._1.Demo.Filters;
using Wjire.ASP.NET.Core3._1.Demo.JWT;
using Wjire.Common;

namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// 操作请求日志记录
    /// </summary>
    public class OperationLogAttribute : ActionFilterAttribute
    {

        private readonly ICapPublisher _capBus;

        public OperationLogAttribute(ICapPublisher capBus)
        {
            _capBus = capBus;
        }


        public override async void OnResultExecuted(ResultExecutedContext context)
        {
            LoginUser loginUser = context.HttpContext.GetLoginUser();
            OperationLog operationLog = new OperationLog
            {
                IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                RouteInfo = context.GetControllerAndAction(),
                Account = loginUser == null ? string.Empty : loginUser.Account,
                RequestInfo = await context.HttpContext.GetRequestBody(),
            };

            if (context.Result is OkObjectResult okObjectResult)
            {
                object okValue = okObjectResult.Value;
                operationLog.ResponseInfo = okValue.ToJson();
            }
            await _capBus.PublishAsync("OperationLog", operationLog);
        }
    }
}
