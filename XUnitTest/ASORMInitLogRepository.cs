using System;
using System.Collections.Generic;
using System.Text;
using Wjire.Db;
using Wjire.Db.Extension;

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
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM ASORMInitLog WHERE CreatedAt > @date ");
            return GetList<ASORMInitLog>(sb.ToString(), new { date = DateTime.Now.AddHours(-6) });
        }


        public ASORMInitLog GetModel(int id, string appName)
        {
            ClearParameters();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM ASORMInitLog WHERE 1=1 ");
            sb.Append(" AND ID=@id ".If(id > 0));
            sb.Append(" AND AppName like @appName".If(string.IsNullOrWhiteSpace(appName) == false));
            sb.Append(" AND CreatedAt > @date");
            return GetSingle<ASORMInitLog>(sb.ToString(), new { id, appName = "%" + appName + "%", date = DateTime.Now.AddHours(-6), money = 1.1M });
        }
    }
}
