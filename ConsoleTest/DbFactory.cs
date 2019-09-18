﻿using wjire;
using Wjire.Db;

namespace ConsoleTest
{

    /// <summary>
    /// 数据库链接工厂
    /// </summary>
    public static class DbFactory
    {
        private static readonly string MagicTaskRecordRead = "MagicTaskRecordRead";
        private static readonly string MagicTaskRecordWrite = "MagicTaskRecordWrite";

        #region 工作单元

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork CreateTransaction()
        {
            return new UnitOfWork(MagicTaskRecordWrite);
        }

        /// <summary>
        /// 创建单链接
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork CreateSingle()
        {
            return new SingleConnection(MagicTaskRecordRead);
        }

        #endregion

        
        #region IASORMInitLogRepository

        /// <summary>
        /// 创建 IASORMInitLogRepository 读链接
        /// </summary>
        /// <returns></returns>
        public static IASORMInitLogRepository CreateIASORMInitLogRepositoryRead()
        {
            return new ASORMInitLogRepository(MagicTaskRecordRead);
        }

        /// <summary>
        /// 创建 IASORMInitLogRepository 读链接
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IASORMInitLogRepository CreateIASORMInitLogRepositoryRead(IUnitOfWork unit)
        {
            return new ASORMInitLogRepository(unit);
        }

        /// <summary>
        /// 创建 IASORMInitLogRepository 写链接
        /// </summary>
        /// <returns></returns>
        public static IASORMInitLogRepository CreateIASORMInitLogRepositoryWrite()
        {
            return new ASORMInitLogRepository(MagicTaskRecordWrite);
        }

        /// <summary>
        /// 创建 IASORMInitLogRepository 写链接
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IASORMInitLogRepository CreateIASORMInitLogRepositoryWrite(IUnitOfWork unit)
        {
            return new ASORMInitLogRepository(unit);
        }


        #endregion
    }
}