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
        private readonly IDbConnection _connection;


        /// <summary>
        /// IDbCommand
        /// </summary>
        public IDbCommand Command { get; private set; }



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">链接名称</param>
        public SingleConnection(string name)
        {
            _connection = ConnectionFactory.GetConnection(name);
            Command = _connection.CreateCommand();
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

            if (_connection == null)
            {
                return;
            }

            _connection.Close();
            _connection.Dispose();
        }
    }
}