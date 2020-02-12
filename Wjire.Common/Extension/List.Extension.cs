using System;
using System.Collections.Generic;
using System.Data;

namespace Wjire.Common.Extension
{

    /// <summary>
    /// List 扩展方法
    /// </summary>
    public static partial class ObjectExtension
    {

        /// <summary>
        /// List 转 DataTable
        /// </summary>
        /// <typeparam name="T">源数据类型</typeparam>
        /// <param name="entities">源数据</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentNullException("entities is null or empty");
            }

            Type type = typeof(T);
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (System.Reflection.PropertyInfo propertyInfo in properties)
            {
                dataTable.Columns.Add(propertyInfo.Name);
            }

            foreach (T obj in entities)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                {
                    objArray[index] = properties[index].GetValue(obj, null);
                }

                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }


        public static bool IsEmpty<T>(this List<T> source)
        {
            return source == null || source.Count == 0;
        }
    }
}
