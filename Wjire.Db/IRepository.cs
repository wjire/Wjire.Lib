using System;

namespace Wjire.Db
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, new()
    {

    }
}
