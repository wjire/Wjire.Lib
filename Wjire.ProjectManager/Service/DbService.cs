using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Wjire.ProjectManager.Model;

namespace Wjire.ProjectManager.Service
{
    public class DbService
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;


        /// <summary>
        /// 读取所有的项目
        /// </summary>
        /// <returns></returns>
        public List<AppInfo> GetAllAppInfo()
        {
            string sql = "SELECT * FROM AppInfo WHERE ServerAddress=@ServerAddress";
            var param = new {ServerAddress = System.Configuration.ConfigurationManager.AppSettings["uploadApi"]};
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                return db.Query<AppInfo>(sql,param).ToList();
            }
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Add(AppInfo info)
        {
            int res = 0;
            string sql = "INSERT INTO AppInfo VALUES (@AppId,@AppName,@AppType,@LocalPath,@ServerAddress)";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                res = db.Execute(sql, info);
            }
            if (res != 1)
            {
                throw new Exception("添加项目失败");
            }
        }


        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public void Delete(long? appId = null)
        {
            int res = 0;
            string sql = "DELETE FROM AppInfo ";
            if (appId.HasValue)
            {
                sql += $" WHERE AppId = {appId}";
            }
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                res = db.Execute(sql);
            }

            if (res == 0)
            {
                throw new Exception("删除项目失败");
            }
        }




        /// <summary>
        /// 更新项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Update(AppInfo info)
        {
            int res = 0;
            string sql = "UPDATE AppInfo SET LocalPath=@LocalPath WHERE AppId=@AppId";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                res = db.Execute(sql, info);
            }

            if (res != 1)
            {
                throw new Exception("更新项目失败");
            }
        }
    }
}
