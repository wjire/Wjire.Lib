using System;
using System.Collections.Generic;

namespace Wjire.Db
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, new()
    {

    }
}
