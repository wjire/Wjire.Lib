using System;

namespace Wjire.Log.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogService.WriteException(new Exception("test"), "测试日志记录");
            LogService.WriteCall("method");
            LogService.WriteText("测试日志记录");

            LogService.WriteExceptionAsync(new Exception("test"), "测试日志记录异步");
            LogService.WriteCallAsync("method asc");
            LogService.WriteTextAsync("测试日志记录异步");

            Console.ReadKey();
        }
    }
}
