
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wjire.Log
{
    /// <summary>
    /// 写入文本日志
    /// </summary>
    public static class LogService
    {




        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <param name="relativePath">路径</param>
        public static void WriteException(Exception ex, string remark, object request = null, object response = null, string relativePath = "Logs/ExceptionLog")
        {
            var logInfo = new ExceptionLogInfo(ex, remark, request, response, relativePath);
            LogCollection.WriteLog(logInfo);
        }


        /// <summary>
        /// 记录调用日志
        /// </summary>
        /// <param name="method">调用方法(必填)</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <param name="relativePath">保存文件夹</param>
        public static void WriteCall(string method, object request = null, object response = null, string relativePath = "Logs/CallLog")
        {
            var logInfo = new CallLogInfo(method, request, response, relativePath);
            LogCollection.WriteLog(logInfo);
        }


        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">文本</param>
        /// <param name="relativePath">保存文件夹</param>
        public static void WriteText(string content, string relativePath = "Logs/TextLog")
        {
            var logInfo = new TextLogInfo(content, relativePath);
            LogCollection.WriteLog(logInfo);
        }


        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <param name="relativePath">路径</param>
        public static void WriteExceptionAsync(Exception ex, string remark, object request = null, object response = null, string relativePath = "Logs/ExceptionLog")
        {
            var logInfo = new ExceptionLogInfo(ex, remark, request, response, relativePath);
            LogCollection.Add(logInfo);
        }


        /// <summary>
        /// 记录调用日志
        /// </summary>
        /// <param name="method">调用方法(必填)</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <param name="relativePath">保存文件夹</param>
        public static void WriteCallAsync(string method, object request = null, object response = null, string relativePath = "Logs/CallLog")
        {
            var logInfo = new CallLogInfo(method, request, response, relativePath);
            LogCollection.Add(logInfo);
        }


        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">文本</param>
        /// <param name="relativePath">保存文件夹</param>
        public static void WriteTextAsync(string content, string relativePath = "Logs/TextLog")
        {
            var logInfo = new TextLogInfo(content, relativePath);
            LogCollection.Add(logInfo);
        }
    }
}