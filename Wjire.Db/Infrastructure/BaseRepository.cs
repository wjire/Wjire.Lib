
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Wjire.Db.Container;

namespace Wjire.Db
{
    /// <summary>
    /// 底层基础处理仓储
    /// </summary>
    public abstract partial class BaseRepository<TEntity> : IDisposable where TEntity : class, new()
    {

        protected static string TableName = typeof(TEntity).Name;


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


        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        protected void AddParameter(string name, object value, ParameterDirection direction = ParameterDirection.Input, int size = 0, byte scale = 0)
        {
            IDbDataParameter param = CreateParameter(name, value, direction, size, scale);
            _cmd.Parameters.Add(param);
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
        protected void AddParameter(string name, object value, DbType type, ParameterDirection direction = ParameterDirection.Input,
            int size = 0, byte scale = 0)
        {
            IDbDataParameter param = CreateParameter(name, value, type, direction, size, scale);
            _cmd.Parameters.Add(param);
        }


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
            param.Value = value ?? string.Empty;
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


        /// <summary>
        /// 清除参数
        /// </summary>
        protected void ClearParameters()
        {
            _cmd.Parameters.Clear();
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
            _cmd.CommandText = sql;
            _cmd.CommandType = type;
            _cmd.CommandTimeout = timeout;
            return _cmd.ExecuteReader(behavior);
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
            _cmd.CommandText = sql;
            _cmd.CommandType = type;
            _cmd.CommandTimeout = timeout;
            return _cmd.ExecuteNonQuery();
        }



        #region 便捷操作


        #region 查询单条

        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>Model or Null</returns>
        protected T GetSingle<T>(string sql) where T : class, new()
        {
            return ExecuteReader(sql).ToModel<T>();
        }


        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param">参数</param>
        /// <returns>Model or Null</returns>
        protected T GetSingle<T>(string sql, object param) where T : class, new()
        {
            AddParameter(param);
            return ExecuteReader(sql).ToModel<T>();
        }


        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>Model or Null</returns>
        protected TEntity GetSingle(string sql)
        {
            return ExecuteReader(sql).ToModel<TEntity>();
        }


        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">参数</param>
        /// <returns>Model or Null</returns>
        protected TEntity GetSingle(string sql, object param)
        {
            AddParameter(param);
            return ExecuteReader(sql).ToModel<TEntity>();
        }


        #endregion


        #region 查询多条

        /// <summary>
        /// 查询多条记录
        /// </summary>w
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns>非 Null</returns>
        protected List<T> GetList<T>(string sql) where T : class, new()
        {
            return ExecuteReader(sql).ToList<T>();
        }


        /// <summary>
        /// 查询多条记录
        /// </summary>w
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param">参数</param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns>非 Null</returns>
        protected List<T> GetList<T>(string sql, object param) where T : class, new()
        {
            AddParameter(param);
            return ExecuteReader(sql).ToList<T>();
        }


        /// <summary>
        /// 查询多条记录
        /// </summary>w
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns>非 Null</returns>
        protected List<TEntity> GetList(string sql)
        {
            return ExecuteReader(sql).ToList<TEntity>();
        }


        /// <summary>
        /// 查询多条记录
        /// </summary>w
        /// <param name="sql"></param>
        /// <param name="param">参数</param>
        /// <param name="type"></param>
        /// <param name="behavior"></param>
        /// <param name="timeout"></param>
        /// <returns>非 Null</returns>
        protected List<TEntity> GetList(string sql, object param)
        {
            AddParameter(param);
            return ExecuteReader(sql).ToList<TEntity>();
        }


        #endregion


        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回受影响的行数</returns>
        public int Add(object entity)
        {
            ClearParameters();
            AddParameter(entity);
            string sql = GetAddSql(entity);
            return ExecuteNonQuery(sql);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramList"></param>
        /// <returns></returns>
        protected string GetWhereIn<T>(IList<T> paramList) where T : struct
        {
            if (paramList == null || paramList.Count == 0)
            {
                throw new ArgumentNullException(nameof(paramList));
            }
            return paramList.Count == 1 ? $" = {paramList[0]} " : $" IN ({string.Join(",", paramList)}) ";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        protected string GetWhereIn(IList<string> paramList)
        {
            if (paramList == null || paramList.Count == 0)
            {
                throw new ArgumentNullException(nameof(paramList));
            }
            return paramList.Count == 1 ? $" = '{paramList[0]}' " : $" IN ('{string.Join("','", paramList)}') ";
        }


        #endregion



        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="param">参数</param>
        private void AddParameter(object param)
        {
            PropertyInfo[] propertyInfos = TypeContainer.GetPropertyInfos(param.GetType());
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                AddParameter(propertyInfo.Name, propertyInfo.GetValue(param));
            }
        }


        /// <summary>
        /// 获取数据库插入数据时的sql语句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetAddSql(object obj)
        {
            Type type = obj.GetType();
            string sql = TypeContainer.AddSqlContainer.GetOrAdd(type, t =>
            {
                //查询表主键
                string queryKey =
                    $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = '{TableName}'";
                string key = ExecuteScalar(queryKey).ToString();
                StringBuilder sqlBuilder = new StringBuilder(128);
                sqlBuilder.Append($" INSERT INTO {TableName} ");

                StringBuilder addBuilder = new StringBuilder(64);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    //忽略主键
                    if (property.Name == key)
                    {
                        continue;
                    }
                    addBuilder.Append($"@{property.Name},");
                }
                string paramString = addBuilder.Remove(addBuilder.Length - 1, 1).ToString();
                string fieldString = paramString.Replace('@', ' ');
                sqlBuilder.Append($"({fieldString}) VALUES ({paramString});");
                return sqlBuilder.ToString();
            });
            return sql;
        }


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