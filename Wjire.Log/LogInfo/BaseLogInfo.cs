

using System;
using System.IO;

namespace Wjire.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    internal abstract class BaseLogInfo
    {
        internal string RelativePath { get; set; }
        internal string AbsolutePath => GetAbsolutePath(RelativePath);

        public override string ToString()
        {
            var result = DateTime.Now.ToString("日志时间:yyyy-MM-dd HH:mm:ss") + Environment.NewLine + ToContent() +
                         Environment.NewLine;
            return result;
        }


        protected abstract string ToContent();


        protected string GetAbsolutePath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
    }
}
