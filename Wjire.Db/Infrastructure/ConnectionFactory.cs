using System;
using System.Data;
using System.Data.SqlClient;

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
                throw new ArgumentNullException("未指定要使用的连接字符串");
            }

            string connectionString = ConnectionStringHelper.GetConnectionString(name);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("未找到指定的连接字符串");
            }

            IDbConnection connection = CreateConnection(connectionString);
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
    }
}
