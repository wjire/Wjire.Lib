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
            return GetList();
            //return ExecuteReader(sql).ToList<ASORMInitLog>();
        }
    }
}
