using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            IDaoqiang dao = new ProductA();
            var child = dao as ProductChild;
            Console.WriteLine(child == null);
        }
    }

    public class ProductChild
    {

    }

    public interface IProduct
    {

    }

    public class DaoqiangBase : ProductChild, IDaoqiang
    {

    }

    public interface IDaoqiang
    {

    }

    public class ProductA : DaoqiangBase
    {

    }

    public class Person : IEquatable<Person>
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public override string ToString()
        {
            return $"{Id}-{Date}";
        }

        /// <inheritdoc />
        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Course
    {
        public int PersonId { get; set; }
        public DateTime Date { get; set; }

        public string CourseName { get; set; }

        public override string ToString()
        {
            return $"{PersonId}-{Date}-{CourseName}";
        }
    }
}


