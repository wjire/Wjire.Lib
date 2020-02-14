
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
        /// 默认的异常日志记录路径
        /// </summary>
        private const string ExceptionLogPath = "Logs/ExceptionLog";

        /// <summary>
        /// 默认的调用日志记录路径
        /// </summary>
        private const string CallLogPath = "Logs/CallLog";

        /// <summary>
        /// 默认的文本日志记录路径
        /// </summary>
        private const string TextLogPath = "Logs/TextLog";


        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <param name="path">路径</param>
        public static Task WriteException(Exception ex, string remark, object request = null, object response = null, string path = ExceptionLogPath)
        {
            return Task.Run(() =>
            {
                string exceptionContent = CreateExceptionContent(ex, remark, request, response);
                WriteLog(exceptionContent, path);
            });
        }


        /// <summary>
        /// 记录调用日志
        /// </summary>
        /// <param name="method">调用方法(必填)</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <param name="path">保存文件夹</param>
        public static Task WriteCall(string method, object request = null, object response = null, string path = CallLogPath)
        {
            return Task.Run(() =>
            {
                string callLog = CreateCallLogContent(method, request, response);
                WriteLog(callLog, path);
            });
        }


        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">文本</param>
        /// <param name="path">保存文件夹</param>
        public static Task WriteText(string content, string path = TextLogPath)
        {
            return Task.Run(() =>
            {
                WriteLog(content, path);
            });
        }


        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="path">日志路径</param>
        private static void WriteLog(string content, string path)
        {
            try
            {
                LogInfo log = new LogInfo
                {
                    Info = DateTime.Now.ToString("日志时间:yyyy-MM-dd HH:mm:ss") + Environment.NewLine + content + Environment.NewLine,
                    Path = GetLogPath(path)
                };
                LogCollection.Add(log);
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 创建异常消息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        /// <returns>结果</returns>
        private static string CreateExceptionContent(Exception ex, string remark, object request, object response)
        {
            StringBuilder stringBuilder = new StringBuilder(512);
            stringBuilder.Append("************************Exception Start********************************");
            string newLine = Environment.NewLine;
            stringBuilder.Append(newLine);
            stringBuilder.AppendLine("Exception Remark：" + remark);
            stringBuilder.AppendLine("Exception Request：" + (request == null ? null : JsonConvert.SerializeObject(request)));
            stringBuilder.AppendLine("Exception Response：" + (response == null ? null : JsonConvert.SerializeObject(response)));
            Exception innerException = ex.InnerException;
            stringBuilder.AppendFormat("Exception Date:{0}{1}", DateTime.Now, Environment.NewLine);
            if (innerException != null)
            {
                stringBuilder.AppendFormat("Inner Exception Type:{0}{1}", innerException.GetType(), newLine);
                stringBuilder.AppendFormat("Inner Exception Message:{0}{1}", innerException.Message, newLine);
                stringBuilder.AppendFormat("Inner Exception Source:{0}{1}", innerException.Source, newLine);
                stringBuilder.AppendFormat("Inner Exception StackTrace:{0}{1}", innerException.StackTrace, newLine);
            }
            stringBuilder.AppendFormat("Exception Type:{0}{1}", ex.GetType(), newLine);
            stringBuilder.AppendFormat("Exception Message:{0}{1}", ex.Message, newLine);
            stringBuilder.AppendFormat("Exception Source:{0}{1}", ex.Source, newLine);
            stringBuilder.AppendFormat("Exception StackTrace:{0}{1}", ex.StackTrace, newLine);
            stringBuilder.Append("************************Exception End************************************");
            stringBuilder.Append(newLine);
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 创建调用信息
        /// </summary>
        /// <param name="method">调用方法</param>
        /// <param name="request">入参</param>
        /// <param name="response">返回值</param>
        private static string CreateCallLogContent(string method, object request, object response)
        {
            StringBuilder sb = new StringBuilder(512);
            sb.Append("************************Start********************************");
            sb.Append(Environment.NewLine);
            sb.AppendFormat("Method：{0}{1}", method, Environment.NewLine);
            sb.AppendFormat("Request:{0}{1}", request == null ? string.Empty : JsonConvert.SerializeObject(request), Environment.NewLine);
            sb.AppendFormat("Response:{0}{1}", response == null ? string.Empty : JsonConvert.SerializeObject(response), Environment.NewLine);
            sb.Append("************************End************************************");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }


        private static string GetLogPath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

    }
}