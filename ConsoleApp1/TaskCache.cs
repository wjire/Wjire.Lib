using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TaskCache<TKey, TResult> : ConcurrentDictionary<TKey, Waiter<TResult>>
    {
        private static readonly TaskCache<TKey, TResult> Singleton = new TaskCache<TKey, TResult>();

        private TaskCache()
        {

        }

        public static TaskCache<TKey, TResult> GetSingleton()
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
