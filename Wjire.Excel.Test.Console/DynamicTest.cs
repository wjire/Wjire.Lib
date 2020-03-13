using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Wjire.Excel.Test.Console
{
    public static class DynamicTest
    {
        public static void Test()
        {
            System.Console.WriteLine("动态为类型添加属性");
            dynamic dynamicModel = new MyDynamic();
            List<string> myList = new List<string>();
            myList.Add("Name");
            myList.Add("Age");
            myList.Add("Hobby");

            List<string> myValueList = new List<string>();
            myValueList.Add("Mary");
            myValueList.Add("18");
            myValueList.Add("Dance");

            for (int i = 0; i < myList.Count; i++)
            {
                //dynamicModel.PropertyName = myList[i];
                //dynamicModel.PropertyValue = myValueList[i];

                dynamicModel.Add(myList[i], myValueList[i]);
            }

            dynamicModel.Id = 1;
            
            //System.Console.WriteLine($"Name: {dynamicModel.Name}");
            //System.Console.WriteLine($"Age: {dynamicModel.Age}");
            //System.Console.WriteLine($"Hobby: {dynamicModel.Hobby}");

            System.Console.WriteLine(dynamicModel.Count);
            foreach (var item in dynamicModel.DicProperty)
            {
                System.Console.WriteLine(item.Key + ":" + item.Value);
            }
        }
    }
}
