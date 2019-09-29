using System.Collections.Generic;
using System.Threading.Tasks;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.DbService
{
    public interface IDbService
    {

        ConnectionInfo ConnectionInfo { get; set; }


        /// <summary>
        /// 构造连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateConnectionString();


        /// <summary>
        /// 获取所有数据库名
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetAllDataBase();


        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetTableNames();


        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<List<TableInfo>> GetTableInfo(string tableName);
    }
}
