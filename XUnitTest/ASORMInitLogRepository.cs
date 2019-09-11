using System.Collections.Generic;
using System.Text;
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


        public ASORMInitLog GetModel(int id)
        {
            ClearParameters();
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.Append(" WHERE ID = @id ");
            AddParameter("id", id);
            string sql = $"SELECT * FROM ASORMInitLog {whereBuilder}";
            return GetModel<ASORMInitLog>(sql);
        }
    }
}
