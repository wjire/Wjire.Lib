using System;

namespace Wjire.ASP.NET.Core3._1.Demo
{
    /// <summary>
    /// ExceptionLog
    /// </summary>
    public class ExceptionLog
    {

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }


        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }


        /// <summary>
        /// 路由
        /// </summary>
        public string RouteInfo { get; set; }


        /// <summary>
        /// 入参
        /// </summary>
        public string RequestInfo { get; set; } = string.Empty;


        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// 异常详情
        /// </summary>
        public string Exception { get; set; }


        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;


    }
}
