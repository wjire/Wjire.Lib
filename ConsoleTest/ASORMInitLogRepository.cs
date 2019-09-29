using System.Collections.Generic;
using Wjire.Db;

namespace wjire
{

    /// <summary>
    /// ASORMInitLogRepository
    /// </summary>
    public class ASORMInitLogRepository : BaseRepository<ASORMInitLog>, IASORMInitLogRepository
    {
        public ASORMInitLogRepository(string name) : base(name) { }

        public ASORMInitLogRepository(IUnitOfWork unit) : base(unit) { }

        #region 便捷操作

        public int Add(ASORMInitLog entity)
        {
            ClearParameters();
            AddParameter(entity);
            string sql = GetInsertSql(entity);
            return ExecuteNonQuery(sql);
        }


        public List<ASORMInitLog> GetAll(List<int> param)
        {
            ClearParameters();
            string sql = $"SELECT * FROM {TableName} WHERE id in {GetWhereIn(param)}";
            return ExecuteReader(sql).ToList<ASORMInitLog>();
        }

        #endregion
    }
}
