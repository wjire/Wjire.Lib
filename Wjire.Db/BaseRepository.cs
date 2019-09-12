
using System;
using System.Collections.Generic;
using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 底层基础处理仓储主类
    /// </summary>
    public abstract class BaseRepository : IDisposable
    {

        /// <summary>
        /// IDbConnection
        /// </summary>
        protected readonly IDbConnection Connection;
        

        /// <summary>
        /// IDbCommand
        /// </summary>
        private readonly IDbCommand _cmd;


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="name">链接名称</param>
        protected BaseRepository(string name)
        {
            if (Connection == null)
            {
                Connection = ConnectionFactory.GetConnection(name);
            }

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            _cmd = Connection.CreateCommand();
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        protected BaseRepository(IUnitOfWork unit)
        {
            if (unit == null)
            {
                return;
            }
            _cmd = unit.Command;
        }


        #region AddParameter


        public BaseRepository AddParameter(Func<bool> func, string name, object value, ParameterDirection direction = ParameterDirection.Input, int size = 0, byte scale = 0)
        {
            return func() ? AddParameter(name, value, direction, size, scale) : this;
        }




        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public BaseRepository AddParameter(string name, object value, ParameterDirection direction = ParameterDirection.Input, int size = 0, byte scale = 0)
        {
            IDbDataParameter param = CreateParameter(name, value, direction, size, scale);
            _cmd.Parameters.Add(param);
            return this;
        }


        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public BaseRepository AddParameter(string name, object value, DbType type, ParameterDirection direction = ParameterDirection.Input,
            int size = 0, byte scale = 0)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction, size, scale);
            _cmd.Parameters.Add(param);
            return this;
        }


        /// <summary>
        /// 清除参数
        /// </summary>
        public BaseRepository ClearParameters()
        {
            _cmd.Parameters.Clear();
            return this;
        }

        #endregion

        #region CreateParameter


        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private IDbDataParameter CreateParameter(string name, object value, ParameterDirection direction, int size, byte scale)
        {
            IDbDataParameter param = _cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.Size = size;
            param.Scale = scale;
            return param;
        }


        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction, int size, byte scale)
        {
            IDbDataParameter param = CreateParameter(name, value, direction, size, scale);
            param.DbType = type;
            return param;
        }


        #endregion


        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected IDataReader ExecuteReader(string sql, CommandType type = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, int timeout = 0)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            _cmd.CommandText = sql;
            _cmd.CommandType = type;
            _cmd.CommandTimeout = timeout;
            IDataReader result = _cmd.ExecuteReader(behavior);
            return result;
        }

        /// <summary>
        /// ExecuteTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected DataTable ExecuteTable(string sql, CommandType type = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, int timeout = 0)
        {
            using (IDataReader dr = ExecuteReader(sql, type, behavior, timeout))
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }


        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected DataSet ExecuteDataSet(string sql, CommandType type = CommandType.Text, params string[] tableName)
        {
            using (IDataReader dr = ExecuteReader(sql, type))
            {
                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.Upsert, tableName);
                return ds;
            }
        }


        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="type">type</param>
        /// <param name="timeout">timeout</param>
        /// <returns>result</returns>
        protected object ExecuteScalar(string sql, CommandType type = CommandType.Text, int timeout = 0)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            _cmd.CommandText = sql;
            _cmd.CommandType = type;
            _cmd.CommandTimeout = timeout;
            object result = _cmd.ExecuteScalar();

            return result == DBNull.Value ? null : result;
        }


        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string sql, CommandType type = CommandType.Text, int timeout = 0)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            _cmd.CommandText = sql;
            _cmd.CommandType = type;
            _cmd.CommandTimeout = timeout;
            int result = _cmd.ExecuteNonQuery();

            return result;
        }



        #region 便捷操作


        /// <summary>
        /// 查询多条记录
        /// </summary>w
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns>非 Null</returns>
        public List<T> GetList<T>(string sql, CommandType type = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, int timeout = 0) where T : class, new()
        {
            return ExecuteReader(sql, type, behavior, timeout).ToList<T>();
        }


        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns>Model or Null</returns>
        public T GetModel<T>(string sql, CommandType type = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, int timeout = 0) where T : class, new()
        {
            return ExecuteReader(sql, type, behavior, timeout).ToModel<T>();
        }

        #endregion



        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _cmd?.Dispose();

            if (Connection == null)
            {
                return;
            }

            Connection.Close();
            Connection.Dispose();
        }
    }
}