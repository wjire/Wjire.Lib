using System;
using Wjire.CodeBuilder;

namespace FileService.SqlCreater
{
    public class SqlCreaterFactory
    {
        public static ITableSqlCreater Create(string type)
        {
            switch (type.ToLower())
            {
                case "sql":
                    return new SqlServerTableSqlCreater();
                case "mysql":
                    return new MySqlTableSqlCreater();
                default:
                    throw new Exception();
            }
        }
    }
}
