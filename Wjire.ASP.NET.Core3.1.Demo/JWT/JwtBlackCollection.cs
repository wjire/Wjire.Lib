using System;
using Wjire.Common;


namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// Jwt黑名单
    /// </summary>
    public class JwtBlackCollection
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="jti"></param>
        public void Add(string jti)
        {
            long stamp = DateTimeHelper.ConvertToTimeStamp(DateTime.Now);
            RedisHelper.ZAdd(JwtConst.JwtBlackCollection, (stamp, jti));
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="jti"></param>
        /// <returns></returns>
        public bool IsExists(string jti)
        {
            long? rank = RedisHelper.ZRank(JwtConst.JwtBlackCollection, jti);
            return rank != null;
        }
    }
}
