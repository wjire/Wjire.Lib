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

        public ASORMInitLog Query(string sql)
        {
            return GetSingle(sql);
        }
        

        public List<ASORMInitLog> QueryList(string sql)
        {
            return GetList(sql);
        }
        

        public T Query<T>(string sql) where T : class, new()
        {
            return GetSingle<T>(sql);
        }
        

        public List<T> QueryList<T>(string sql) where T : class, new()
        {
            return GetList<T>(sql);
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
