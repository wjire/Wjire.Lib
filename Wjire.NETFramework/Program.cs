using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Wjire.NETFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            SqlConnection conn = new SqlConnection();
            conn.Dispose();
            List<Person> list = new List<Person>
            {
                new Person{ Logins = new List<Login>
                {
                    new Login{ Date = DateTime.Now.AddDays(1)},
                    new Login{ Date = DateTime.Now.AddDays(-1)},
                    new Login{ Date = DateTime.Now},
                }
                }
            };


            list.ForEach(f => f.Logins.ForEach(ff => Console.WriteLine(ff.Date)));

            Console.WriteLine();

            list.ForEach(f => f.Logins = f.Logins.OrderBy(o => o.Date).ToList());
            list.ForEach(f => f.Logins.ForEach(ff => Console.WriteLine(ff.Date)));
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
