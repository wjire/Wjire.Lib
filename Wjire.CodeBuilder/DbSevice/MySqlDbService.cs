using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.DbService
{

    /// <summary>
    /// MySqlDbService
    /// </summary>
    public class MySqlDbService : BaseDbService, IDbService
    {

        public MySqlDbService(ConnectionInfo info) : base(info)
        {
            ConnectionString = CreateConnectionString();
        }


        /// <summary>
        /// 构造连接字符串
        /// </summary>
        /// <returns></returns>
        public string CreateConnectionString()
        {
            return
                $"server={ConnectionInfo.IP};port=3306;" +
                $"user id={ConnectionInfo.User};password={ConnectionInfo.Pwd};" +
                $"persistsecurityinfo=True;" +
                $"database={ConnectionInfo.DbName};" +
                $"charset=utf8;AllowUserVariables=True";
        }


        /// <summary>
        /// 获取所有数据库名
        /// </summary>
        /// <returns></returns>
        public Task<List<string>> GetAllDataBase()
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = new MySqlConnection(ConnectionString))
                {
                    return connection.Query<string>("show databases").ToList();
                }
            });
        }


        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <returns></returns>
        public Task<List<string>> GetTableNames()
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = new MySqlConnection(ConnectionString))
                {
                    return connection.Query<string>($"select table_name from information_schema.tables where table_schema='{ConnectionInfo.DbName}'").ToList();
                }
            });
        }



        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Task<List<TableInfo>> GetTableInfo(string tableName)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = new MySqlConnection(ConnectionString))
                {
                    return connection.Query<TableInfo>(GetSql(tableName)).ToList();
                }
            });
        }


        private string GetSql(string tableName)
        {
            return string.Format(Sql, ConnectionInfo.DbName, tableName);
        }


        private const string Sql = @"
                                    select
                                        COLUMN_NAME as ColumnName,
                                        DATA_TYPE as ColumnType,
                                        COLUMN_COMMENT as ColumnDescription,
                                        case IS_NULLABLE
                                            when 'NO' then 0
                                            when 'YES' then 1
                                            else 0
                                        end IsNullable,
                                        case COLUMN_KEY
                                            when 'PRI' then 1
                                        else 0
                                        end IsKey
                                    from information_schema.columns
                                    where table_schema = '{0}' #表所在数据库
                                    and table_name = '{1}' ; #你要查的表
                                    ";
    }
}
