using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Throttle.Model
{
    public abstract class ThrottleBase
    {
        /// <summary>
        /// 每秒鐘處理量的上限 (平均值)
        /// </summary>
        protected double RateLimit;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">指定服務量上限</param>
        protected ThrottleBase(double rate)
        {
            this.RateLimit = rate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount">Request 的處理量</param>
        /// <param name="exec"></param>
        /// <returns>傳回是否受理 request 的結果。
        /// true: 受理, 會在一定時間內處理該 request;
        /// false: 不受理該 request</returns>
        public abstract bool ProcessRequest(int amount, Action exec = null);
    }
}
