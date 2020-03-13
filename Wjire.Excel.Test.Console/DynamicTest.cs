using System.Collections.Generic;

namespace Wjire.Excel.Test.Console
{
    public static class DynamicTest
    {
        public static void Test()
        {
            System.Console.WriteLine("动态为类型添加属性");
            dynamic dynamicModel = new MyDynamic();
            List<string> myList = new List<string>
            {
                "Name",
                "Age",
                "Hobby"
            };

            List<string> myValueList = new List<string>
            {
                "Mary",
                "18",
                "Dance"
            };

            for (int i = 0; i < myList.Count; i++)
            {
                dynamicModel.PropertyName = myList[i];
                dynamicModel.PropertyValue = myValueList[i];
            }

            dynamicModel.Id = 1;

            //System.Console.WriteLine($"Name: {dynamicModel.Name}");
            //System.Console.WriteLine($"Age: {dynamicModel.Age}");
            //System.Console.WriteLine($"Hobby: {dynamicModel.Hobby}");

            System.Console.WriteLine(dynamicModel.Count);

            foreach (dynamic item in dynamicModel.DicProperty)
            {
                System.Console.WriteLine(item.Key + ":" + item.Value);
            }
        }
    }
}
