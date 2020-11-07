using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wjire.Throttle.Model
{
    public class DummyThrottle : ThrottleBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">指定服務量上限</param>
        public DummyThrottle(double rate) : base(rate)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount">Request 的處理量</param>
        /// <param name="exec"></param>
        /// <returns>傳回是否受理 request 的結果。
        /// true: 受理, 會在一定時間內處理該 request;
        /// false: 不受理該 request</returns>
        public override bool ProcessRequest(int amount, Action exec)
        {
            Task.Run(exec);
            return true;
        }
    }
}
