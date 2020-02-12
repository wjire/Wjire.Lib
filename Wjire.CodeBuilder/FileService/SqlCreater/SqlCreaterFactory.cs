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
                case "sqlserver":
                    return new SqlServerTableSqlCreater();
                case "mysql":
                    return new MySqlTableSqlCreater();
                default:
                    throw new Exception("尚不支持 "+type);
            }
        }
    }
}
