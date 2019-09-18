using System.Collections.Generic;
using Wjire.Db;

namespace ConsoleTest
{

    /// <summary>
    /// ASORMInitLogRepository
    /// </summary>
    public class ASORMInitLogRepository : BaseRepository<ASORMInitLog>, IASORMInitLogRepository
    {
        public ASORMInitLogRepository(string name) : base(name)
        {
        }

        public ASORMInitLogRepository(IUnitOfWork unit) : base(unit)
        {
        }

        #region 便捷操作

        public int Add(ASORMInitLog log)
        {
            ClearParameters();
            AddParameter(log);
            string sql = GetInsertSql(log);
            return ExecuteNonQuery(sql);
        }

        public ASORMInitLog Query(string sql)
        {
            return ExecuteReader(sql).ToModel<ASORMInitLog>();
        }

        public ASORMInitLog Query(string sql, object param)
        {
            ClearParameters();
            AddParameter(param);
            return ExecuteReader(sql).ToModel<ASORMInitLog>();
        }

        public List<ASORMInitLog> QueryList(string sql)
        {
            return ExecuteReader(sql).ToList<ASORMInitLog>();
        }

        public List<ASORMInitLog> QueryList(string sql, object param)
        {
            ClearParameters();
            AddParameter(param);
            return ExecuteReader(sql).ToList<ASORMInitLog>();
        }

        public T Query<T>(string sql) where T : class, new()
        {
            return ExecuteReader(sql).ToModel<T>();
        }

        public T Query<T>(string sql, object param) where T : class, new()
        {
            ClearParameters();
            AddParameter(param);
            return ExecuteReader(sql).ToModel<T>();
        }

        public List<T> QueryList<T>(string sql) where T : class, new()
        {
            return ExecuteReader(sql).ToList<T>();
        }

        public List<T> QueryList<T>(string sql, object param) where T : class, new()
        {
            ClearParameters();
            AddParameter(param);
            return ExecuteReader(sql).ToList<T>();
        }

        #endregion
    }
}
