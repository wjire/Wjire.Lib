using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wjire.Throttle.Model
{
    public class LeakyBucketThrottle : ThrottleBase
    {
        private readonly TimeSpan _timeWindow;
        private bool flag = true;
        private int count;
        private Stopwatch timer;
        private object locker = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">指定服務量上限</param>
        public LeakyBucketThrottle(double rate, TimeSpan timeWindow) : base(rate)
        {
            _timeWindow = timeWindow;
            timer = new Stopwatch();
            timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount">Request 的處理量</param>
        /// <param name="exec"></param>
        /// <returns>傳回是否受理 request 的結果。
        /// true: 受理, 會在一定時間內處理該 request;
        /// false: 不受理該 request</returns>
        public override bool ProcessRequest(int amount, Action exec = null)
        {
            lock (locker)
            {
                if (count + amount < RateLimit * timer.ElapsedMilliseconds / 1000)
                {
                    count += amount;
                    Task.Run(exec);
                    return true;
                }
                return false;
            }
        }
    }
}
