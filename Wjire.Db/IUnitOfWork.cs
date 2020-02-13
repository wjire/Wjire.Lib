using System;
using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        IDbCommand Command { get; }

        IDbTransaction Transaction { get; }

        void Commit();

        void Rollback();
    }
}
