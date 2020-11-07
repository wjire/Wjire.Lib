using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Wjire.Throttle.Model;

namespace Wjire.Throttle
{
    class Program
    {
        static void Main(string[] args)
        {
            ThrottleBase t =
            //new DummyThrottle(500);
            //new CounterThrottle(500, TimeSpan.FromSeconds(5));
            //new CounterThrottle(500, TimeSpan.FromSeconds(1));
            //new StatisticEngineThrottle(500, TimeSpan.FromSeconds(5));
            //new StatisticEngineThrottle(500, TimeSpan.FromSeconds(1));
            new LeakyBucketThrottle(500, TimeSpan.FromSeconds(5));
            //new LeakyBucketThrottle(500, TimeSpan.FromSeconds(1));
            //new TokenBucketThrottle(500, TimeSpan.FromSeconds(5));
            //new TokenBucketThrottle(500, TimeSpan.FromSeconds(1));


            int statistic_success = 0;
            int statistic_fail = 0;

            int statistic_execute = 0;
            long statistic_executeTime = 0;

            //
            //  製造 (平均) 200qps 穩定的流量
            //  產生多個 threads, 用穩定的速度 (會加上亂數打散) 產生 request(s), 交給 throttle 處理。
            //
            List<Thread> threads = new List<Thread>();
            bool stop = false;
            bool idle = false;
            for (int i = 0; i < 30; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Random rnd = new Random();
                    while (stop == false)
                    {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();

                        if (idle)
                        {

                        }
                        else if (t.ProcessRequest(1, () =>
                        {
                            Interlocked.Increment(ref statistic_execute);
                            Interlocked.Add(ref statistic_executeTime, timer.ElapsedMilliseconds);
                        }))
                        {
                            Interlocked.Increment(ref statistic_success);
                        }
                        else
                        {
                            Interlocked.Increment(ref statistic_fail);
                        }
                        Thread.Sleep(rnd.Next(100));
                    }
                });
                thread.Start();
                threads.Add(thread);
            }


            //
            //
            //  製造 peek 的流量 (約 700 ~ 100 qps)
            //  產生1個 threads, 每隔 17 sec, 就有 2 sec 會產生大量 request 交給 throttle 處理。用以測試尖峰流量的處理效果。
            //
            {
                Thread thread = new Thread(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    while (stop == false)
                    {
                        timer.Restart();
                        while (timer.ElapsedMilliseconds < 2000)
                        {
                            Stopwatch _timer = new Stopwatch();
                            _timer.Start();

                            if (idle)
                            {

                            }
                            else if (t.ProcessRequest(1, () =>
                            {
                                Interlocked.Increment(ref statistic_execute);
                                Interlocked.Add(ref statistic_executeTime, _timer.ElapsedMilliseconds);
                            }))
                            {
                                Interlocked.Increment(ref statistic_success);
                            }
                            else
                            {
                                Interlocked.Increment(ref statistic_fail);
                            }
                            Thread.Sleep(1);
                            //SpinWait.SpinUntil(() => false, 5);
                        }
                        Task.Delay(15000).Wait();
                    }
                });
                thread.Start();
                threads.Add(thread);
            }
            //
            //  製造離峰的流量, 每 21 秒約 3 秒沒有任何流量
            //
            {
                Thread thread = new Thread(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    while (stop == false)
                    {
                        idle = true;
                        timer.Restart();
                        SpinWait.SpinUntil(() => timer.ElapsedMilliseconds >= 3000);
                        idle = false;

                        Task.Delay(18000).Wait();
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            //
            //  產生一個 thread, 定期每秒分別統計:
            //  0. 所有發出的 request 總數 => (1) + (2)
            //  1. 成功受理的 request 總數
            //  2. 拒絕受理的 request 總數
            //  3. 已執行的 request 總數
            //
            {
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine($"TotalRequests,SuccessRequests,FailRequests,ExecutedRequests,AverageExecuteTime");
                    while (stop == false)
                    {
                        //Console.WriteLine("{0} per sec", Interlocked.Exchange(ref statistic_success, 0));

                        int success = Interlocked.Exchange(ref statistic_success, 0);
                        int fail = Interlocked.Exchange(ref statistic_fail, 0);
                        int exec = Interlocked.Exchange(ref statistic_execute, 0);
                        long exectime = Interlocked.Exchange(ref statistic_executeTime, 0);

                        double avgExecTime = 0;
                        if (exec > 0) avgExecTime = 1.0D * exectime / exec;

                        Console.WriteLine($"{success + fail},{success},{fail},{exec},{avgExecTime}");
                        Task.Delay(1000).Wait();
                    }
                });
                thread.Start();
                threads.Add(thread);
            }


            Thread.Sleep(1000 * 120);
            Console.WriteLine("Shutdown...");

            stop = true;

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
