using System;
using System.Collections.Generic;
using Wjire.Log;

namespace Wjire.NETFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogService.WriteException(new Exception("123123"), "remark");
            Console.ReadKey();
        }
    }

    internal class Person
    {
        public List<Login> Logins { get; set; }
    }

    internal class Login
    {
        public DateTime Date { get; set; }
    }
}
