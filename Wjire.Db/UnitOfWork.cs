using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 事务操作
    /// </summary>	
    public class UnitOfWork : IUnitOfWork
    {

        /// <summary>
        /// IDbTransaction
        /// </summary>
        private readonly IDbTransaction _transaction;


        /// <summary>
        /// IDbConnection
        /// </summary>
        public IDbConnection Connection { get; }



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
            _transaction = isolationLevel.HasValue ? Connection.BeginTransaction(isolationLevel.Value) : Connection.BeginTransaction();
        }



        /// <summary>
        /// 提交事物
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();

            Command?.Dispose();

            if (Connection == null)
            {
                return;
            }

            Connection.Close();
            Connection.Dispose();
        }
    }
}
