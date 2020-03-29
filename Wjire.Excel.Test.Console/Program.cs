using System.Collections.Generic;
using System.Dynamic;

namespace Wjire.Excel.Test.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //            DynamicTest.Test();

            string path = @"C:\Users\Administrator\Desktop\testexcel.xlsx";
            //var dt = EPPlusHelper.WorksheetToTable(path);
            //ExcelWriteHelper.CreateFile(dt, @"C:\Users\Administrator\Desktop\11.xlsx");


            //ExcelReadHandler handler = new ExcelReadHandler(path);

            //var columns = handler.GetCells(1);
            //System.Console.WriteLine(string.Join(",", columns));

            //List<Person> persons = handler.Read<Person>();

            List<Person> persons = new List<Person>
            {
                new Person
                {
                    Account = "wjire",
                    Name = "wjire",
                    Address = "1",
                    Age = 15,
                    CompanyName = "wjireCompanyName"
                },
                new Person
                {
                    Account = "wjire",
                    Name = "wjire",
                    Address = "1",
                    Age = 15,
                    CompanyName = "wjireCompanyName"
                }
            };

            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                {"Account", "登录账号"},
                {"Age", "芳龄"}
            };
            ExcelWriteHelper.CreateFile<Person>(persons, path, dic);


            System.Console.WriteLine("over");
            System.Console.ReadKey();
        }
    }

    public class MyDynamic : DynamicObject
    {
        public string PropertyName { get; set; }

        // The inner dictionary.
        public Dictionary<string, object> DicProperty { get; } = new Dictionary<string, object>();


        // This property returns the number of elements
        // in the inner dictionary.
        public int Count => DicProperty.Count;

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name;

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return DicProperty.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            if (binder.Name == "PropertyValue")
            {
                DicProperty[PropertyName] = value;
            }
            else
            {
                DicProperty[binder.Name] = value;
            }


            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

        public void Add(string name, object value)
        {
            DicProperty[name] = value;
        }
    }
}
