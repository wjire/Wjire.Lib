using System;
using System.IO;
using System.Linq;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = @"c:\1\2\3\temp\1.0.0.0";
            string[] arr = path.Split(".");
            int number = Convert.ToInt32(arr[arr.Length - 1]);
            number += 1;
            arr[arr.Length - 1] = number.ToString();
            string newVer = string.Join(".", arr);
            var version = new Version("1.0.0.0");
            Console.WriteLine(newVer);
        }
    }
}
