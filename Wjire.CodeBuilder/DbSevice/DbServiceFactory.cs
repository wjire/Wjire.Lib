using System;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.DbService
{
    public static class DbServiceFactory
    {

        public static IDbService CreateDbService(ConnectionInfo info)
        {
            if (info.Type == "sqlserver")
            {
                return new SqlDbService(info);
            }
            else if (info.Type == "mysql")
            {
                return new MySqlDbService(info);
            }
            throw new ArgumentException($"尚不支持该数据库类型 : {info.Type}");
        }
    }
}
