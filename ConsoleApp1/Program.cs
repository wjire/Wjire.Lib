using System;
using System.Collections.Generic;
using Wjire.Excel;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<Person>
            {
                new Person {Id = 1, Name = "wjire1"},
                new Person {Id = 2, Name = "wjire2"},
                new Person {Id = 3, Name = "wjire3"},
            };
            var path = @"C:\Users\Administrator\Desktop\1.csv";
            ExcelHelper.CreateFile(list,path);
            Console.WriteLine("Hello World!");
        }
    }
}
