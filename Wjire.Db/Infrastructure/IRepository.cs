using System;
using System.Collections.Generic;

namespace Wjire.Db.Infrastructure
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        int Add(TEntity entity);
        TEntity GetSingle(string sql);
        List<TEntity> GetList(string sql);
        T GetSingle<T>(string sql) where T : class, new();
        List<T> GetList<T>(string sql) where T : class, new();
    }
}
