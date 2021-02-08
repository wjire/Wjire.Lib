using System;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace ConsoleApp1
{
    class Program
    {
        private static Random random = new Random();

        static async Task Main(string[] args)
        {
            //var cts = new CancellationTokenSource();
            //cts.CancelAfter(TimeSpan.FromSeconds(2));
            //AsyncManualResetEvent Semaphore = new AsyncManualResetEvent();
            //Task.Run(async () =>
            //{
            //    await Task.Delay(TimeSpan.FromSeconds(3));
            //    Semaphore.Set();
            //});
            //try
            //{
            //    await Semaphore.WaitAsync(cts.Token);
            //    Console.WriteLine("等待完成");
            //}
            //catch (OperationCanceledException e)
            //{
            //    Console.WriteLine(e);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            var cache = TaskCache<int, int>.GetSingleton();
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
                            await waiter.WaitAsync(TimeSpan.FromSeconds(2));
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
