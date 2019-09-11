
using System;
using System.Data;

namespace Wjire.Db
{
    /// <summary>
    /// 底层基础处理仓储
    /// </summary>
    public abstract partial class BaseRepository : IDisposable
    {

        /// <summary>
        /// IDbConnection
        /// </summary>
        private readonly IDbConnection _connection;


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
            if (_connection == null)
            {
                _connection = ConnectionFactory.GetConnection(name);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            _cmd = _connection.CreateCommand();
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
                _connection = unit.Connection;
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

        #region  ExecuteReader

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, CommandType type, CommandBehavior behavior, int timeout)
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
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, CommandType type, CommandBehavior behavior)
        {
            return ExecuteReader(sql, type, behavior, 0);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, CommandType type, int timeout)
        {
            return ExecuteReader(sql, type, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, CommandType type)
        {
            return ExecuteReader(sql, type, CommandBehavior.Default, 0);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, CommandBehavior behavior, int timeout)
        {
            return ExecuteReader(sql, CommandType.Text, behavior, timeout);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, CommandBehavior behavior)
        {
            return ExecuteReader(sql, CommandType.Text, behavior, 0);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql, int timeout)
        {
            return ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        protected IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, 0);
        }

        #endregion

        #region ExecuteTable

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, CommandType type, CommandBehavior behavior, int timeout)
        {
            using (IDataReader dr = ExecuteReader(sql, type, behavior, timeout))
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, CommandType type, CommandBehavior behavior)
        {
            return ExecuteTable(sql, type, behavior, 0);
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, CommandType type, int timeout)
        {
            return ExecuteTable(sql, type, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, CommandType type)
        {
            return ExecuteTable(sql, type, CommandBehavior.Default, 0);
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, CommandBehavior behavior, int timeout)
        {
            return ExecuteTable(sql, CommandType.Text, behavior, timeout);
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, CommandBehavior behavior)
        {
            return ExecuteTable(sql, CommandType.Text, behavior, 0);
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql, int timeout)
        {
            return ExecuteTable(sql, CommandType.Text, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// The execute table.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        protected DataTable ExecuteTable(string sql)
        {
            return ExecuteTable(sql, CommandType.Text, CommandBehavior.Default, 0);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// The execute data set.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        protected DataSet ExecuteDataSet(string sql, params string[] tableName)
        {
            return ExecuteDataSet(sql, CommandType.Text, tableName);
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
        protected DataSet ExecuteDataSet(string sql, CommandType type, params string[] tableName)
        {
            using (IDataReader dr = ExecuteReader(sql, type, CommandBehavior.Default, 0))
            {
                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.Upsert, tableName);
                return ds;
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="type">type</param>
        /// <param name="timeout">timeout</param>
        /// <returns>result</returns>
        protected object ExecuteScalar(string sql, CommandType type, int timeout)
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
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>result</returns>
        protected object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, CommandType.Text, 0);
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        protected int ExecuteNonQuery(string sql, CommandType type, int timeout)
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

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        protected int ExecuteNonQuery(string sql, CommandType type)
        {
            return ExecuteNonQuery(sql, type, 0);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        protected int ExecuteNonQuery(string sql, int timeout)
        {
            return ExecuteNonQuery(sql, CommandType.Text, timeout);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        protected int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, CommandType.Text, 0);
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _cmd?.Dispose();

            if (_connection == null)
            {
                return;
            }

            _connection.Close();
            _connection.Dispose();
        }
    }
}