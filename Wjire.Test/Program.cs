using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSRedis;

namespace Wjire.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var i in GetInt())
            {
                Console.WriteLine(i);
            }
        }

        static IEnumerable<int> GetInt()
        {
            var result = Enumerable.Range(1, 100);
            foreach (var i in result)
            {
                yield return i;
            }
        }
    }
}
