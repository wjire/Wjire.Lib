using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        /*
         * 问题:
         * 一个线程加载数据,加载的同时把数据压入管道.与此同时,另外一个线程在管道的接收端接收并处理数据,并且是先进先出.
         *
         * 这其实就是生产者和消费者
         * 但这种方式比较适合有独立的线程作为生产者或者消费者,比如线程池线程.
         * 如果要以异步的方式访问管道,例如UI线程作为消费者,阻塞队列就不太合适了,而应该用异步队列.
         */
        static async Task Main(string[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(new { OrderId = 123 }));
            var queue = new BlockingCollection<int>(boundedCapacity: 10);//入参表示队列的容量,如果消费速度慢了,容量达到上限,则生产者线程会阻塞.
            {
                //实际上 BlockingCollection 默认实现的是 阻塞队列,它还有下面两种:
                var stack = new BlockingCollection<int>(new ConcurrentStack<int>());//阻塞栈,先进后出
                var bag = new BlockingCollection<int>(new ConcurrentQueue<int>());//阻塞包,无序
            }

            StartConsumer1Async(queue);
            StartConsumer2Async(queue);  //起两个消费者,则是按劳消费,由于消费者2每次都会暂停1秒,所以被消费者1全部消费了

            //主线程加载数据,并压入管道.
            var rnd = new Random();
            while (true)
            {
                queue.Add(rnd.Next(1, 100));
                queue.Add(rnd.Next(1, 100));
                queue.Add(rnd.Next(1, 100));
                queue.Add(rnd.Next(1, 100));
                queue.Add(rnd.Next(1, 100));
                if (queue.Count > 5)
                {
                    Console.WriteLine("生产者休息会");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }

            queue.CompleteAdding();

            //ConsumeByTake(collection);
            Console.ReadKey();
        }


        private static void StartConsumer1Async(BlockingCollection<int> collection)
        {
            Task.Run(() =>
            {
                foreach (var data in collection.GetConsumingEnumerable())
                {
                    Console.WriteLine($"线程{Thread.CurrentThread.ManagedThreadId}消费了 {data}");
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
            });
        }

        private static void StartConsumer2Async(BlockingCollection<int> collection)
        {
            Task.Run(() =>
            {
                foreach (var data in collection.GetConsumingEnumerable())
                {
                    Console.WriteLine($"线程{Thread.CurrentThread.ManagedThreadId}消费了 {data}");
                    Thread.Sleep(TimeSpan.FromSeconds(0.3));
                }
            });
        }

        //使用 Take 1条1条消费
        private static void ConsumeByTake(BlockingCollection<int> collection)
        {
            while (collection.TryTake(out var item))
            {
                Console.WriteLine($"消费了 {item}");
            }
        }

    }
}