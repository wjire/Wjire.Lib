using System;
using System.Collections.Generic;
using Wjire.Excel;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var list = new List<Person>
            //{
            //    new Person {Id = 1, Name = "wjire1"},
            //    new Person {Id = 2, Name = "wjire2"},
            //    new Person {Id = 3, Name = "wjire3"},
            //};
            //var path = @"C:\Users\Administrator\Desktop\1.csv";
            //ExcelWriteHelper.CreateFile(list,path);
            string path = @"C:\Users\Administrator\Desktop\123.csv";
            //var lists = new ExcelReadHandler(path).Read<Temp>(new[] { "licenseplate", "time", "speed", "lng" });
            List<Temp> lists = CsvReadHelper.ReadByGB2312<Temp>(path);
            foreach (Temp item in lists)
            {
                Console.WriteLine(item.licenseplate);
                Console.WriteLine(item.lng);
                Console.WriteLine(item.speed);
                Console.WriteLine(item.time);
            }
            Console.WriteLine("Hello World!");
        }
    }


    public class Temp
    {
        public string licenseplate { get; set; }
        public string time { get; set; }
        public string speed { get; set; }
        public string lng { get; set; }
    }
}
