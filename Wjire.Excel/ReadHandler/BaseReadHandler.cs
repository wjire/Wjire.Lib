using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Wjire.Excel
{
    public class BaseReadHandler
    {
        protected SortedDictionary<int, string> GetColumnMaps(Type type)
        {
            SortedDictionary<int, string> result = new SortedDictionary<int, string>();
            foreach (PropertyInfo info in type.GetProperties())
            {
                DisplayAttribute displayAttribute = info.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                {
                    //throw new Exception($"{info.Name} 属性未定义 DisplayAttribute");
                    continue;
                }
                result.Add(displayAttribute.Order, info.Name);
            }
            return result;
        }
    }
}
