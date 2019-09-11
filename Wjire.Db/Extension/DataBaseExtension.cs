using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Wjire.Db.Container;

namespace Wjire.Db
{

    /// <summary>
    /// 数据库扩展方法
    /// </summary>
    public static class DataBaseExtension
    {

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            List<T> result = new List<T>();
            try
            {
                DataTable dt = reader.GetSchemaTable();
                if (dt == null)
                {
                    return result;
                }

                Type type = typeof(T);
                while (reader.Read())
                {
                    T t = new T();
                    foreach (DataRow dr in dt.Rows)
                    {
                        // 当前列名&属性名
                        string columnName = dr[0].ToString();
                        PropertyInfo pro = TypeContainer.GetProperty(type, columnName);

                        if (pro == null)
                        {
                            continue;
                        }
                        pro.SetValue(t, ConvertHelper(reader[columnName], pro.PropertyType), null);
                    }
                    result.Add(t);
                }
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }

            return result;
        }


        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            List<T> result = new List<T>();
            Type type = typeof(T);
            if (dt.Rows.Count <= 0)
            {
                return result;
            }
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    // 当前列名&属性名
                    string columnName = column.ColumnName;
                    PropertyInfo pro = TypeContainer.GetProperty(type, columnName);

                    if (pro == null)
                    {
                        continue;
                    }

                    pro.SetValue(t, ConvertHelper(dr[columnName], pro.PropertyType), null);
                }
                result.Add(t);
            }

            return result;
        }


        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataSet ds) where T : class, new()
        {
            return ds.Tables[0].ToList<T>();
        }


        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <param name="dataTableIndex">dataTableIndex</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataSet ds, int dataTableIndex) where T : class, new()
        {
            return ds.Tables[dataTableIndex].ToList<T>();
        }


        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this IDataReader reader) where T : class, new()
        {
            T t = new T();
            try
            {
                DataTable dt = reader.GetSchemaTable();
                if (dt == null)
                {
                    return t;
                }
                Type type = typeof(T);
                while (reader.Read())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        // 当前列名&属性名
                        string columnName = dr[0].ToString();
                        PropertyInfo pro = TypeContainer.GetProperty(type, columnName);

                        if (pro == null)
                        {
                            continue;
                        }
                        pro.SetValue(t, ConvertHelper(reader[columnName], pro.PropertyType), null);
                    }
                }
            }

            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }

            return t;
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this DataTable dt) where T : class, new()
        {
            T t = new T();
            if (dt.Rows.Count <= 0)
            {
                return t;
            }
            Type type = typeof(T);
            foreach (DataColumn column in dt.Columns)
            {
                // 当前列名&属性名
                string columnName = column.ColumnName;
                PropertyInfo pro = TypeContainer.GetProperty(type, columnName);
                if (pro == null)
                {
                    continue;
                }
                pro.SetValue(t, ConvertHelper(dt.Rows[0][columnName], pro.PropertyType), null);
            }
            return t;
        }


        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <param name="dataTableIndex">dataTableIndex</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this DataSet ds, int dataTableIndex = 0) where T : class, new()
        {
            return ds.Tables[0].ToModel<T>();
        }




        private static object ConvertHelper(object value, Type conversionType)
        {
            Type nullableType = Nullable.GetUnderlyingType(conversionType);

            // 判断当前类型是否可为 null
            if (nullableType != null)
            {
                if (value == DBNull.Value)
                {
                    return null;
                }

                //若是枚举 则先转换为枚举
                if (nullableType.IsEnum)
                {
                    value = Enum.Parse(nullableType, value.ToString());
                }

                return Convert.ChangeType(value, nullableType);
            }

            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}
