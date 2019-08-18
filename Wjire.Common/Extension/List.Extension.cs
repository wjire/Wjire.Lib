﻿using System;
using System.Collections.Generic;
using System.Data;

namespace Wjire.Common.Extension
{

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
            if (entities == null || entities.Count < 1)
            {
                throw new ArgumentNullException("需转换的集合为空");
            }

            Type type = entities[0].GetType();
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
    }
}
