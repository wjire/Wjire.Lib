using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Wjire.Core.Test
{
    public class HttpCache<TKey, TResult> : ConcurrentDictionary<TKey, Waiter<TResult>>
    {
        private static readonly HttpCache<TKey, TResult> Singleton = new HttpCache<TKey, TResult>();

        private HttpCache()
        {

        }

        public static HttpCache<TKey, TResult> GetSingleton()
        {
            return Singleton;
        }

        public Waiter<TResult> GetWaiterAsync(TKey key, Func<Task<TResult>> func)
        {
            if (Singleton.TryGetValue(key, out var waiter) == true)
            {
                if (waiter.Task.IsCompleted)
                {
                    var s = "";
                    if (waiter.Task.IsFaulted)
                    {
                        s = "，失败了";
                    }
                    Console.WriteLine($"{key}:请求完成" + s);
                    TryRemove(key, out var cache1);
                }
                else if (waiter.Task.IsCanceled)
                {
                    TryRemove(key, out var cache1);
                }
                return waiter;
            }

            Console.WriteLine($"{key} ：开始请求");
            waiter = new Waiter<TResult>();
            Singleton.TryAdd(key, waiter);
            waiter.AddTask(func);
            return waiter;
        }
    }
}
