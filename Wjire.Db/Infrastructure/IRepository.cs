using System;
using System.Collections.Generic;

namespace Wjire.Db.Infrastructure
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        int Add(TEntity entity);
        TEntity Query(string sql);
        TEntity Query(string sql, object param);
        List<TEntity> QueryList(string sql);
        List<TEntity> QueryList(string sql, object param);
        T Query<T>(string sql) where T : class, new();
        T Query<T>(string sql, object param) where T : class, new();
        List<T> QueryList<T>(string sql) where T : class, new();
        List<T> QueryList<T>(string sql, object param) where T : class, new();
    }
}
