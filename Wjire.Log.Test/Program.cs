using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Wjire.Log.Test
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //var source = Enumerable.Range(1, 10);
            // await foreach (var i in Get(source))
            //{
            //    Console.WriteLine(i);
            //}

            await SumFromOneToCountAsyncYield();
        }

        //static async IAsyncEnumerable<int> Get(IEnumerable<int> source)
        //{
        //    foreach (var item in source)
        //    {
        //        yield return item;
        //    }
        //}

        /// <summary>
        /// in this function, the result was splited by servral results and displayed in the consle.
        /// this is the benifit of yield feature. we can get some of the result before we get the whole result.
        /// but we can also see, the producter's logic still block the main thread.
        /// </summary>
        public static async Task SumFromOneToCountAsyncYield()
        {
            const int count = 5;
            Console.WriteLine("Sum with yield starting.");
            await foreach (var i in SumFromOneToCountAsyncYield(count))
            {
                Console.WriteLine($"thread id: {System.Threading.Thread.GetCurrentProcessorId()},  current time: {DateTime.Now}");
                Console.WriteLine($"Yield sum: {i}");
                Console.WriteLine();
            }
            Console.WriteLine("Sum with yield completed.");

            Console.WriteLine("################################################");
            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// 1. make this method to be an async method.
        /// 2. create task.delay to intent a long work for get the sum result.
        /// 3. use yield return to return the temp sum value to customer.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async static IAsyncEnumerable<int> SumFromOneToCountAsyncYield(int count)
        {
            Console.WriteLine("SumFromOneToCountYield called!");
            var sum = 0;
            for (var i = 0; i <= count; i++)
            {
                sum = sum + i;
                await Task.Delay(TimeSpan.FromSeconds(5));
                yield return sum;
            }
        }
    }
}
