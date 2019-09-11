
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

        protected readonly IDbTransaction Transaction;


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
            if (unit != null)
            {
                Connection = unit.Connection;
                Transaction = unit.Transaction;
                _cmd = unit.Command;
            }
        }


        #region Parameter

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        protected IDbDataParameter AddParameter(string name)
        {
            IDbDataParameter param = CreateParameter(name);
            _cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        protected IDbDataParameter AddParameter(string name, object value)
        {
            IDbDataParameter param = CreateParameter(name, value);
            _cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type)
        {
            IDbDataParameter param = CreateParameter(name, value, type);
            _cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction);
            _cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type, ParameterDirection direction,
            int size)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction, size);
            _cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type, ParameterDirection direction,
            int size, byte scale)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction, size, scale);
            _cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 清除参数
        /// </summary>
        protected void ClearParameters()
        {
            _cmd.Parameters.Clear();
        }

        #endregion

        #region CreateParameter

        /// <summary>
        /// The create parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        private IDbDataParameter CreateParameter(string name)
        {
            IDbDataParameter param = _cmd.CreateParameter();
            param.ParameterName = name;
            return param;
        }

        /// <summary>
        /// The create parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        private IDbDataParameter CreateParameter(string name, object value)
        {
            IDbDataParameter param = CreateParameter(name);
            param.Value = value ?? DBNull.Value;
            return param;
        }

        /// <summary>
        /// The create parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type)
        {
            IDbDataParameter param = CreateParameter(name, value);
            param.DbType = type;
            return param;
        }

        /// <summary>
        /// The create parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            IDbDataParameter param = CreateParameter(name, value, type);
            param.Direction = direction;
            return param;
        }

        /// <summary>
        /// The create parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction,
            int size)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction);
            param.Size = size;
            return param;
        }

        /// <summary>
        /// The create parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/>.
        /// </returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction,
            int size, byte scale)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction, size);
            param.Scale = scale;
            return param;
        }

        #endregion
        

        /// <summary>
        /// 
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
        /// 
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
        /// The execute data set.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
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
        /// 
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

        public List<T> GetList<T>(string sql, CommandType type = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, int timeout = 0) where T : class, new()
        {
            return ExecuteReader(sql, type, behavior, timeout).ToList<T>();
        }


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