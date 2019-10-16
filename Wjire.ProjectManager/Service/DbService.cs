using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Wjire.ProjectManager.Model;

namespace Wjire.ProjectManager.Service
{
    public class DbService
    {
        private readonly string _connectionString;

        public DbService(string connectionString)
        {
            _connectionString = connectionString;
        }



        /// <summary>
        /// 读取所有的项目
        /// </summary>
        /// <returns></returns>
        public List<AppInfo> GetAllAppInfo()
        {
            string sql = "SELECT * FROM AppInfo";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                return db.Query<AppInfo>(sql).ToList();
            }
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddProjectInfo(AppInfo info)
        {
            string sql = "INSERT INTO AppInfo VALUES (@AppId,@AppName,@AppPath,@AppType,@LocalPath)";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                return db.Execute(sql, info);
            }
        }
    }
}
