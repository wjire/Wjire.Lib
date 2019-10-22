
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        private static readonly string ExceptionLogPath = "Logs/ExceptionLog";


        /// <summary>
        /// 默认的文本日志记录路径
        /// </summary>
        private static readonly string TextLogPath = "Logs/TextLog";


        /// <summary>
        /// 记录异常文本日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        public static void WriteLog(Exception ex, string remark)
        {
            WriteLog(ex, remark, null);
        }


        /// <summary>
        /// 记录异常文本日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <param name="request">入参</param>
        public static void WriteLog(Exception ex, string remark, object request)
        {
            WriteLog(ex, remark, request, null);
        }



        /// <summary>
        /// 记录异常文本日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <param name="request">入参</param>
        /// <param name="path">路径</param>
        public static void WriteLog(Exception ex, string remark, object request, string path)
        {
            StringBuilder errorMessage = CreateErrorMessage(ex, remark, request);
            WriteLog(errorMessage.ToString(), path ?? Path.Combine(GetLogPath(), ExceptionLogPath));
        }


        /// <summary>
        /// 创建异常消息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="remark">备注,通常是业务描述或者方法名</param>
        /// <returns>结果</returns>
        /// <param name="request">入参</param>
        private static StringBuilder CreateErrorMessage(Exception ex, string remark, object request)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("************************Exception Start********************************");
            string newLine = Environment.NewLine;
            stringBuilder.Append(newLine);
            stringBuilder.AppendLine("Exception Remark：" + remark);
            stringBuilder.AppendLine("Exception Request：" + JsonConvert.SerializeObject(request));
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
            return stringBuilder;
        }

        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void WriteLog(string content)
        {
            WriteLog(content, Path.Combine(GetLogPath(), TextLogPath));
        }

        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="path">日志路径</param>
        public static void WriteLog(string content, string path)
        {
            Task.Run(() => Log(content, path));
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="method">调用方法(必填)</param>
        /// <param name="request">请求参数</param>
        /// <param name="response">输出参数</param>
        /// <param name="saveFolder">保存文件夹，默认为CallLog</param>
        public static void SaveLog(string method, object request, object response, string saveFolder = "CallLog")
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("************************Start********************************");
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.AppendFormat("Method：{0}{1}", method, Environment.NewLine);
                stringBuilder.AppendFormat("Request:{0}{1}", request == null ? string.Empty : JsonConvert.SerializeObject(request), Environment.NewLine);
                stringBuilder.AppendFormat("Response:{0}{1}", response == null ? "void" : JsonConvert.SerializeObject(response), Environment.NewLine);
                stringBuilder.Append("************************End************************************");
                stringBuilder.Append(Environment.NewLine);
                string logContent = stringBuilder.ToString();
                string path = Path.Combine(GetLogPath(), "Logs/" + saveFolder);
                WriteLog(logContent, path);
            }
            catch (Exception ex)
            {
                WriteLog(ex, "记录调用日志异常");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool Log(string content, string path)
        {
            try
            {
                LogInfo log = new LogInfo
                {
                    Info = DateTime.Now.ToString("日志时间:yyyy-MM-dd HH:mm:ss") + Environment.NewLine + content + Environment.NewLine,
                    Path = path
                };
                return TextWriter.WriteLog(log);
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取日志路径
        /// </summary>
        /// <returns>路径</returns>
        private static string GetLogPath()
        {
            string logPath = ConfigureHelper.GetString("LogPath");
            return string.IsNullOrWhiteSpace(logPath) ? AppDomain.CurrentDomain.BaseDirectory : logPath;
        }
    }
}