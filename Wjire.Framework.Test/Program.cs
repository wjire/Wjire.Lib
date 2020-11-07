using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wjire.Framework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Action action = () => Console.WriteLine("hello world");

            //action.Invoke();

            var rr = action.BeginInvoke(r =>
             {
                 if (r.IsCompleted)
                 {
                     Console.WriteLine("ok");
                 }
             }, null);

            while (true)
            {
                if (rr.IsCompleted)
                {
                    break;
                }
            }

            Console.ReadKey();
        }
    }
}
