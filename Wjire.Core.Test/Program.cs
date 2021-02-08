using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Wjire.Core.Test
{
    class Program
    {
        private static Random random = new Random();

        static async Task Main(string[] args)
        {
            var cache = HttpCache<int, int>.GetSingleton();
            for (int i = 0; i < 1; i++)
            {
                var user = i;
                //Task.Run(() =>
                //{
                //    Console.WriteLine(user);
                //});

                await Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            var waiter = cache.GetWaiterAsync(user, GetResult);
                            var r = await waiter.WaitAsync(TimeSpan.FromSeconds(2));
                            if (r == false)
                            {
                                Console.WriteLine($"{user}-本次请求没有拿到数据");
                                await Task.Delay(TimeSpan.FromSeconds(random.Next(3)));
                                continue;
                            }
                            cache.TryRemove(user, out var t);
                            var result = waiter.Task.Result;
                            Console.WriteLine($"{user}-拿到数据了：" + result + ",耗时:" + waiter.Ts.TotalMilliseconds + " 毫秒");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(random.Next(3)));
                    }

                });
            }

            Console.ReadKey();
        }

        static Task<int> GetResult()
        {
            var task = Task.Run(() =>
            {
                var number = random.Next(10);
                if (number <= 3)
                {
                    throw new Exception("抛出异常");
                }
                var time = TimeSpan.FromSeconds(random.Next(10));
                Thread.Sleep(time);
                return new Random().Next(1, 100);
            });
            return task;
        }
    }
}
