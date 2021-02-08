using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Wjire.Redis.RedLock.Demo.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            var result = Parallel.For(0, 20, (i) =>
             {
                 var stopwatch = new Stopwatch();
                 stopwatch.Start();
                 var response = client.GetAsync($"http://localhost:5000/locktest").Result;
                 stopwatch.Stop();
                 var data = response.Content.ReadAsStringAsync().Result;
                 Console.WriteLine($"ThreadId:{Thread.CurrentThread.ManagedThreadId}, Result:{data}, Time:{stopwatch.ElapsedMilliseconds}");
             });
            client.Dispose();
            Console.ReadKey();
        }
    }
}
