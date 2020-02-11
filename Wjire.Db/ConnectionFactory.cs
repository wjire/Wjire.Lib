using System;
using System.Configuration;
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

            ConnectionStringSettings settings = ConnectionStringHelper.GetConnectionStringSettings(name);
            IDbConnection connection = CreateConnection(settings);
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


        private static IDbConnection CreateConnection(ConnectionStringSettings settings)
        {
            switch (settings.ProviderName.ToLower())
            {
                case "sql":
                    return new SqlConnection(settings.ConnectionString);
                case "mysql":
                    return new MySqlConnection(settings.ConnectionString);
                default: throw new ArgumentException($"尚不支持 {settings.ProviderName} 数据库");
            }
        }
    }
}
