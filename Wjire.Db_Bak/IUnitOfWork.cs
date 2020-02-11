using System;
using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        IDbCommand Command { get; }

        void Commit();

        void Rollback();
    }
}
