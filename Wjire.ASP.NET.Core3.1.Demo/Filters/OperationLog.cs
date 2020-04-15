using System;

namespace Wjire.ASP.NET.Core3._1.Demo.Filters
{
    /// <summary>
    /// OperationLog
    /// </summary>
    public class OperationLog
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
        /// 返回值
        /// </summary>
        public string ResponseInfo { get; set; } = string.Empty;


        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
