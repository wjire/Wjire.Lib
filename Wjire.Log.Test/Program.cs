using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Acb.Core.Helper.Http;
using Acb.Core.Serialize;
using Microsoft.Extensions.DependencyInjection;
using Wjire.Common;
using iText;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Acb.Core.Extensions;
using iText.Pdfa;
using iText.Kernel.Pdf.Canvas.Parser.Util;

namespace Wjire.Log.Test
{
    internal class Program
    {

        private static ServiceProvider provider;

        private static void Main(string[] args)
        {
            var s1 = State1.woman;
            var s2 = Enum.Parse(typeof(State2), s1.ToString());
            Console.WriteLine(s1);
            Console.WriteLine((State2)s2);
        }
    }

    public enum State1
    {
        man = 1,
        woman = 2,
    }

    public enum State2
    {
        man = 2,
        woman = 3
    }

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }


        public string Address1 { get; set; }


        public string Address2 { get; set; }

        public string[] GetPropertyNames<T>()
        {
            return typeof(T).GetProperties().Select(s => s.Name).ToArray();
        }

        public virtual void SetPerson(Person person)
        {

        }
    }

    public class PersonB : Person
    {
        public override void SetPerson(Person person)
        {
            person.Id = 1;
        }
    }

    public class PersonA : Person
    {
        public override void SetPerson(Person person)
        {
            person.Name = "name";
        }
    }

    public class PersonC : Person
    {
        public override void SetPerson(Person person)
        {
            person.Address = "address";
        }
    }

    public class PersonC1 : Person
    {
        public override void SetPerson(Person person)
        {
            person.Address1 = "address1";
        }
    }

    public class PersonC2 : Person
    {
        public override void SetPerson(Person person)
        {
            person.Address2 = "address2";
        }
    }

    public class Animal
    {
        public int Age { get; set; }
    }
}


