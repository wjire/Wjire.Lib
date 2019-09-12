using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Wjire.Db
{

    /// <summary>
    /// SqlHelper
    /// </summary>
    internal static class SqlHelper
    {

        private static readonly ConcurrentDictionary<Type, string> UpdateSqlContainer = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, string> AddSqlContainer = new ConcurrentDictionary<Type, string>();


        /// <summary>
        /// 获取数据库插入数据时的sql语句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string GetAddSql(object obj)
        {
            var type = obj.GetType();
            string sql = AddSqlContainer.GetOrAdd(type, t =>
            {
                StringBuilder sqlBuilder = new StringBuilder(64);
                sqlBuilder.Append($" INSERT INTO {type.Name} ");

                StringBuilder addBuilder = new StringBuilder(64);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    //忽略的属性,不参与构造
                    KeyAttribute att = property.GetCustomAttribute<KeyAttribute>();
                    if (att != null)
                    {
                        continue;
                    }
                    addBuilder.Append($"@{property.Name},");
                }
                string paramString = addBuilder.Remove(addBuilder.Length - 1, 1).ToString();
                string fieldString = paramString.Replace('@', ' ');
                sqlBuilder.Append($"({fieldString}) VALUES ({paramString});");
                return sqlBuilder.ToString();
            });
            return sql;
        }




        /// <summary>
        /// 获取数据库更新数据时的sql语句
        /// </summary>
        /// <param name="obj">需要更新的对象</param>
        /// <returns></returns>
        internal static string GetUpdateSql(object obj)
        {
            Type type = obj.GetType();
            return GetUpdateSql(type, type.Name);
        }


        /// <summary>
        /// 获取数据库更新数据时的sql语句
        /// </summary>
        /// <param name="obj">需要更新的对象</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string GetUpdateSql(object obj, string tableName)
        {
            Type type = obj.GetType();
            tableName = string.IsNullOrWhiteSpace(tableName) ? type.Name : tableName;
            return GetUpdateSql(type, tableName);
        }



        /// <summary>
        /// 获取数据库更新数据时的sql语句
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string GetUpdateSql(Type type, string tableName)
        {
            string result = UpdateSqlContainer.GetOrAdd(type, t =>
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append($" UPDATE {tableName} SET ");

                foreach (PropertyInfo property in type.GetProperties())
                {
                    //忽略的属性,不参与构造
                    KeyAttribute att = property.GetCustomAttribute<KeyAttribute>();
                    if (att != null)
                    {
                        continue;
                    }
                    sqlBuilder.Append($"{property.Name}=@{property.Name},");
                }
                return sqlBuilder.Remove(sqlBuilder.Length - 1, 1).ToString();
            });
            return result;
        }
    }
}
