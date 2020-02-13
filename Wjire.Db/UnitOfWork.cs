using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 事务操作
    /// </summary>	
    public class UnitOfWork : IUnitOfWork
    {

        /// <summary>
        /// IDbConnection
        /// </summary>
        public IDbConnection Connection { get; }


        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction Transaction { get; }



        /// <summary>
        /// IDbCommand
        /// </summary>
        public IDbCommand Command { get; private set; }




        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">链接名称</param>
        /// <param name="isolationLevel">事务级别</param>
        public UnitOfWork(string name, IsolationLevel? isolationLevel = null)
        {
            Connection = ConnectionFactory.GetConnection(name);
            Command = Connection.CreateCommand();
            Transaction = isolationLevel.HasValue ? Connection.BeginTransaction(isolationLevel.Value) : Connection.BeginTransaction();
        }



        /// <summary>
        /// 提交事物
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Transaction?.Dispose();
            Command?.Dispose();
            Connection?.Dispose();
        }
    }
}
