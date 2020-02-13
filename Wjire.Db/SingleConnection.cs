using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 单连接读取数据
    /// </summary>
    public class SingleConnection : IUnitOfWork
    {

        /// <summary>
        /// IDbConnection
        /// </summary>
        public IDbConnection Connection { get; }


        /// <summary>
        /// IDbCommand
        /// </summary>
        public IDbCommand Command { get; private set; }


        /// <summary>
        /// IDbTransaction
        /// </summary>
        public IDbTransaction Transaction { get; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">链接名称</param>
        public SingleConnection(string name)
        {
            Connection = ConnectionFactory.GetConnection(name);
            Command = Connection.CreateCommand();
        }



        /// <summary>
        /// 提交事物
        /// </summary>
        public void Commit()
        {
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public void Rollback()
        {
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Command?.Dispose();
            Connection?.Dispose();
        }
    }
}