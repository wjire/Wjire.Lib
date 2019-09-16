using System;
using System.Collections.Generic;
using Wjire.Common.Extension;
using Wjire.Db;

namespace XUnitTest
{

    /// <summary>
    /// 
    /// </summary>
    public class ASORMInitLogRepository : BaseRepository<ASORMInitLog>
    {
        public ASORMInitLogRepository(string name) : base(name)
        {
        }

        public ASORMInitLogRepository(IUnitOfWork unit) : base(unit)
        {
        }


        public List<ASORMInitLog> GetList()
        {
            ClearParameters();
            string sql = " SELECT * FROM ASORMInitLog WHERE CreatedAt > @date ";
            AddParameter("date", DateTime.Now.AddDays(-1));
            return GetList(sql);
            //return ExecuteReader(sql).ToList<ASORMInitLog>();
        }


        public ASORMInitLog GetModel(int id, string appName)
        {
            ClearParameters();
            string sql = $" SELECT * FROM {TableName} WHERE 1=1 " +
                      $"{" AND ID=@ID ".If(id > 0)} " +
                      $"{" AND AppName like @appName".If(string.IsNullOrWhiteSpace(appName) == false)}" +
                      $"{" AND CreatedAt > @date".If(true)}";
            return GetSingle<ASORMInitLog>(sql, new { id, appName = "%" + appName + "%", date = DateTime.Now.AddHours(-6), money = 1.1M });
        }
    }
}
