using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Wjire.Db
{

    /// <summary>
    /// 连接工厂
    /// </summary>
    internal class ConnectionFactory
    {

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns> 
        internal static IDbConnection GetConnection(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            //string connectionString = ConnectionStringHelper.GetConnectionString(name);
            //if (string.IsNullOrWhiteSpace(connectionString))
            //{
            //    throw new ArgumentException($"未找到连接字符串:{name}");
            //}

            var connectionStringInfo = ConnectionStringHelper.GetConnectionString(name);
            IDbConnection connection = CreateConnection(connectionStringInfo);
            if (connection == null)
            {
                throw new ArgumentException("创建数据库连接失败");
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }


        private static IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }


        private static IDbConnection CreateConnection(ConnectionStringInfo connectionStringInfo)
        {
            switch (connectionStringInfo.Type)
            {
                case "sql":
                    return new SqlConnection(connectionStringInfo.ConnectionString);
                case "mysql":
                    return new MySqlConnection(connectionStringInfo.ConnectionString);
                default: throw new ArgumentException($"尚不支持 {connectionStringInfo.Type} 数据库");
            }
        }
    }
}
