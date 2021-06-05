using System;
using System.Collections.Generic;

namespace Wjire.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> enumerable = CreateSimpleIterator();
            foreach (var item in enumerable)
            {
                if (item == 4)
                {
                    break;
                }
                Console.WriteLine(item);
            }

            //List<IPerson<string>> people = new List<IPerson<string>>();

            //Attribute[] arrs = new Attribute[1];
            //arrs[0] = new MainAttribute();

            //Console.WriteLine(arrs[0].GetType().Name);

            //var a = 1;
            //var b = 3;
            //var c = 4;

            //Console.WriteLine(a & b);
            //Console.WriteLine(b ^ a);
            //Console.WriteLine(a ^ a);

            //var genders = new List<Gender>
            //{
            //    Gender.Default,
            //    Gender.Man
            //};
            //var type = typeof(Gender);
            //var fields = type.GetFields().Where(w => w.GetCustomAttribute<MainAttribute>() != null);

            //var mains = fields.Select(s => (Gender)s.GetRawConstantValue()).ToList();

            //foreach (var gender in genders)
            //{
            //    if (mains.Any(a1 => a1 == gender))
            //    {
            //        Console.WriteLine(gender + " is main");
            //    }
            //    else
            //    {
            //        Console.WriteLine(gender + " is not main");
            //    }
            //}

            Console.ReadKey();
        }

        static IEnumerable<int> CreateSimpleIterator()
        {
            try
            {
                yield return 10;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 2)
                    {
                        yield break;
                    }

                    yield return i;
                }

                yield return 20;
            }
            finally
            {
                Console.WriteLine("enumerable over");
            }
        }

        public void Method<T1>()
        {

        }
        public void Method<T1, T2>() { }

        public delegate void Printer(string message);
        public static void PrintAnything(object obj)
        {
            Console.WriteLine(obj);
        }
    }

    enum Gender
    {
        [Main]
        Man = 1,
        Woman = 2,
        Default = 3,
        [Main]
        Center = 4
    }

    public class MainAttribute : Attribute
    {

    }

    public interface IPerson<out T>
    {

    }

    public struct Person
    {
        public int Id { get; set; }

        public Person(int id)
        {
            Id = id;
        }
    }
}
