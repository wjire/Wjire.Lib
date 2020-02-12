using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Db
{
    public partial class BaseRepository<TEntity>
    {

        /// <summary>
        /// 新增
        /// </summary>
        public virtual void Add(TEntity entity)
        {
            ClearParameters();
            string sql = GetInsertSql(entity);
            AddParameter(entity);
            int addResult = ExecuteNonQuery(sql);
            if (addResult != 1)
            {
                throw new Exception("insert into database throw a exception");
            }
        }
    }
}
