using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.DbService
{

    /// <summary>
    /// SqlDbService
    /// </summary>
    public class SqlDbService : BaseDbService, IDbService
    {

        public SqlDbService(ConnectionInfo info) : base(info)
        {
            ConnectionString = CreateConnectionString();
        }


        /// <summary>
        /// 构造连接字符串
        /// </summary>
        /// <returns></returns>
        public string CreateConnectionString()
        {
            return $"Data Source={ConnectionInfo.IP};Initial Catalog={ConnectionInfo.DbName};User ID={ConnectionInfo.User};PassWord={ConnectionInfo.Pwd};persist security info=True;";
        }


        /// <summary>
        /// 获取所有数据库名
        /// </summary>
        /// <returns></returns>
        public Task<List<string>> GetAllDataBase()
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = new SqlConnection(ConnectionString))
                {
                    return connection.Query<string>("select name from master..sysdatabases").ToList();
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
                using (IDbConnection connection = new SqlConnection(ConnectionString))
                {
                    return connection.Query<string>(@"USE [" + ConnectionInfo.DbName + "] SELECT name FROM sys.sysobjects WHERE type='U'or type='v' ORDER BY name").ToList();
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
                using (IDbConnection connection = new SqlConnection(ConnectionString))
                {
                    List<TableInfo> result = connection.Query<TableInfo>(GetSql(tableName)).ToList();
                    foreach (TableInfo item in result.Where(w => w.IsKey == "1"))
                    {
                        item.IsIncrement = connection.QuerySingleOrDefault<string>(
                            $"SELECT COLUMNPROPERTY( OBJECT_ID('{tableName}'),'{item.ColumnName}','IsIdentity') as IsIdentity");
                    }
                    return result;
                }
            });
        }


        private string GetSql(string tableName)
        {
            return string.Format(Sql, tableName);
        }


        private const string Sql = @"
                                   SELECT
	                                    obj.name AS 表名,
	                                    col.colorder AS 序号,
	                                    col.name AS ColumnName,
	                                    ISNULL( ep.[value], '' ) AS ColumnDescription,
	                                    t.name AS ColumnType,	                                 
                                    CASE		
		                                    WHEN EXISTS (
		                                    SELECT
			                                    1 
		                                    FROM
			                                    dbo.sysindexes si
			                                    INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id 
			                                    AND si.indid = sik.indid
			                                    INNER JOIN dbo.syscolumns sc ON sc.id = sik.id 
			                                    AND sc.colid = sik.colid
			                                    INNER JOIN dbo.sysobjects so ON so.name = si.name 
			                                    AND so.xtype = 'PK' 
		                                    WHERE
			                                    sc.id = col.id 
			                                    AND sc.colid = col.colid 
			                                    ) THEN
			                                    1 ELSE 0
		                                    END AS IsKey,
	                                    CASE			
			                                    WHEN col.isnullable = 1 THEN
			                                    1 ELSE 0 
		                                    END AS IsNullable
	                                    FROM
		                                    dbo.syscolumns col
		                                    LEFT JOIN dbo.systypes t ON col.xtype = t.xusertype
		                                    INNER JOIN dbo.sysobjects obj ON col.id = obj.id 
		                                    AND obj.xtype = 'U' 
		                                    AND obj.status >= 0
		                                    LEFT JOIN dbo.syscomments comm ON col.cdefault = comm.id
		                                    LEFT JOIN sys.extended_properties ep ON col.id = ep.major_id 
		                                    AND col.colid = ep.minor_id 
		                                    AND ep.name = 'MS_Description'
		                                    LEFT JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id 
		                                    AND epTwo.minor_id = 0 
		                                    AND epTwo.name = 'MS_Description' 
	                                    WHERE
		                                    obj.name = '{0}' --表名		
                                    ORDER BY
	                                    col.colorder;
                                    ";
    }
}
