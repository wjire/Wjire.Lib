using System.Collections.Generic;
using Wjire.Dapper;
using Wjire.Db;

namespace XUnitTest
{

    /// <summary>
    /// 
    /// </summary>
    public class ASORMInitLogRepository : DapperBaseRepository
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
            string sql = "SELECT * FROM ASORMInitLog";
            return ExecuteReader(sql).ToList<ASORMInitLog>();
        }


        public ASORMInitLog GetModel(int id, string name)
        {
            ClearParameters();
            AddParameter("1", 1);

            ClearParameters().AddParameter(() => id > 0, "id", id)
                .AddParameter(() => string.IsNullOrWhiteSpace(name) == false, "name", "name");
            string sql = "SELECT * FROM ASORMInitLog WHERE 1=1 AND ID = @id ";
            return GetModel<ASORMInitLog>(sql);
        }
    }
}
