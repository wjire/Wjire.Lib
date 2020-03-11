using System;

namespace Wjire.Excel.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\Administrator\Desktop\1.xlsx";
            var handler = new ExcelReadHandler(path);
            var data = handler.Read<Person>(new[] { "Id", "Name" });
            System.Console.WriteLine(data.Count);
            System.Console.ReadKey();
        }
    }


    class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
