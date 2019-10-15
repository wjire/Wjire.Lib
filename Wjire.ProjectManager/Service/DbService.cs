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
        public List<ProjectInfo> GetProjectInfo()
        {
            string sql = "SELECT * FROM ProjectInfo";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                return db.Query<ProjectInfo>(sql).ToList();
            }
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddProjectInfo(ProjectInfo info)
        {
            string sql = "INSERT INTO ProjectInfo VALUES (@ProjectName,@ProjectDir,@ProjectType)";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                return db.Execute(sql, info);
            }
        }
    }
}
