using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wjire.Log
{
    /// <summary>
    /// 日志容器
    /// </summary>
    internal static class LogCollection
    {

        /// <summary>
        /// 日志集合
        /// </summary>
        private static readonly BlockingCollection<BaseLogInfo> Logs = new BlockingCollection<BaseLogInfo>(int.MaxValue);


        static LogCollection()
        {
            Task.Run(WriteLog);
        }


        /// <summary>
        /// 消费日志
        /// </summary>
        internal static void WriteLog()
        {
            foreach (BaseLogInfo log in Logs.GetConsumingEnumerable())
            {
                WriteLog(log);
            }
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo"></param>
        internal static void WriteLog(BaseLogInfo logInfo)
        {
            try
            {
                DateTime timeStamp = DateTime.Now;
                string path = GetFileMainPath(logInfo.AbsolutePath, timeStamp);
                FileInfo lastFile = GetLastAccessFile(path, timeStamp);
                using (FileStream fileStream = GetFileStream(lastFile, path, timeStamp))
                {
                    if (fileStream == null)
                    {
                        return;
                    }
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.Write(logInfo.ToString());
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="baseLog">日志</param>
        /// <returns>bool</returns>
        internal static void Add(BaseLogInfo baseLog)
        {
            Logs.Add(baseLog);
        }


        /// <summary>
        /// 获取最后写入日志的文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>FileInfo</returns>
        private static FileInfo GetLastAccessFile(string path, DateTime timeStamp)
        {
            FileInfo result = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                result = fileInfos.FirstOrDefault(f => timeStamp.Hour == f.CreationTime.Hour);
            }
            else
            {
                directoryInfo.Create();
            }
            return result;
        }

        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="fileInfo">lastFile</param>
        /// <param name="path">path</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>FileStream</returns>
        private static FileStream GetFileStream(FileInfo fileInfo, string path, DateTime timeStamp)
        {
            FileStream result;
            if (fileInfo == null)
            {
                result = CreateFile(path, GetFileMainName(timeStamp));
            }
            else if (IsOutOfTimeMaxLength(fileInfo.CreationTime, timeStamp))
            {
                result = CreateFile(path, GetFileMainName(timeStamp));
            }
            else
            {
                result = fileInfo.OpenWrite();
            }
            return result;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">名称</param>
        /// <returns>FileStream</returns>
        private static FileStream CreateFile(string path, string fileName)
        {
            return File.Create($@"{path}\{fileName}.log");
        }

        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns></returns>
        private static string GetFileMainName(DateTime timeStamp)
        {
            return timeStamp.ToString("HH");
        }

        /// <summary>
        /// IsOutOfTimeMaxLength
        /// </summary>
        /// <param name="creationTime">creationTime</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>bool</returns>
        private static bool IsOutOfTimeMaxLength(DateTime creationTime, DateTime timeStamp)
        {
            return Math.Abs((creationTime - timeStamp).TotalHours) >= 1;
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>path</returns>
        private static string GetFileMainPath(string path, DateTime timeStamp)
        {
            return Path.Combine(path, timeStamp.ToString("yyyyMMdd"));
        }
    }
}