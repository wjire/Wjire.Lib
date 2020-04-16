using System;

namespace Wjire.Log.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //LogService.WriteException(new Exception("test"), "测试日志记录");
            //LogService.WriteCall("method");
            //LogService.WriteText("测试日志记录");

            //LogService.WriteExceptionAsync(new Exception("test"), "测试日志记录异步");
            //LogService.WriteCallAsync("method asc");
            //LogService.WriteTextAsync("测试日志记录异步");

            var start = new DateTime(2020, 3, 16, 12, 53, 0);
            var end = new DateTime(2020, 3, 18, 9, 2, 0);

            var start1 = Convert.ToDateTime(start.ToShortDateString());
            var end1 = Convert.ToDateTime(end.ToShortDateString());

            Console.WriteLine(end1.Subtract(start1).Days);
            Console.WriteLine((end1-start1).Days);

            Console.ReadKey();
        }
    }
}
