using System;
using System.Collections.Generic;

namespace Wjire.Db.Infrastructure
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        int Add(TEntity entity);
        TEntity Query(string sql);
        List<TEntity> QueryList(string sql);
        T Query<T>(string sql) where T : class, new();
        List<T> QueryList<T>(string sql) where T : class, new();
    }
}
