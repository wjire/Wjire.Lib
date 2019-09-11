using Dapper;
using System.Collections.Generic;
using System.Linq;
using Wjire.Db;

namespace Wjire.Dapper
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class DapperBaseRepository : BaseRepository
    {
        protected DapperBaseRepository(string name) : base(name)
        {
        }

        protected DapperBaseRepository(IUnitOfWork unit) : base(unit)
        {

        }


        public int Add<T>(T model, string tableName)
        {
            return Connection.Execute($"INSERT INTO {tableName} VALUES(@AppID,@CreatedAt,@AppName,@PackageName,@DeviceNo,@DeviceBrand,@UnitType,@DeviceOsVer,@HasSim,@MAC,@IP,@BootTime,@IsCharging,@RemainingBattery,@ScreenActivation)", model);
        }


        public List<T> GetListUseDapper<T>(string sql, object param)
        {
            return Connection.Query<T>(sql, param, Transaction).ToList();
        }


        public T GetModelUseDapper<T>(string sql, object param) where T : class, new()
        {
            return Connection.QueryFirstOrDefault<T>(sql, param, Transaction);
        }
    }
}
