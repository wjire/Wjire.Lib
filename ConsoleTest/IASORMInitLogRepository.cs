using System.Collections.Generic;
using Wjire.Db;

namespace wjire
{
    /// <summary>
    /// IASORMInitLogRepository
    /// </summary>
    public interface IASORMInitLogRepository : IRepository<ASORMInitLog>
    {
        List<ASORMInitLog> GetAll(List<int> param);
    }
}
