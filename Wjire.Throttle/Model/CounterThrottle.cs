using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Wjire.Throttle.Model
{

    /*
     * 这种算法的原理是控制总量,假设目标是 500 rps,也就是说只要访问量不超过 500 rps * 单位时间 就可以了.
     * 这个单位时间,可以是 1s ,也可以是 5s
     * 这个算法的缺点是:
     * 你无法掌控 time window 范围内(单位时间内)的表现。设的过短，你会花很多运算能力在服务量控制这件事上。设的过长，实际访问量会出现远大于 500 rps 的情况.
     * 总的来说就是,服务量掌控的不够精准，后果就是后端的服务可能在瞬间被打垮，而垮掉之后又有一段时间内没有request ..
     *
     * 还有个边界的问题.比如
     * 本来是0-5秒这个时间段的请求,但是计算的时候,刚好超过了5秒,就会算到6-10秒这个时间段.
     *
     *
     */
    public class CounterThrottle : ThrottleBase
    {
        private readonly TimeSpan _timeWindow;

        private double _counter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">指定服務量上限</param>
        /// <param name="timeWindow"></param>
        public CounterThrottle(double rate, TimeSpan timeWindow) : base(rate)
        {
            _timeWindow = timeWindow;
            Task.Run(() =>
            {
                Stopwatch timer = new Stopwatch();
                while (true)
                {
                    this._counter = 0;
                    timer.Restart();
                    SpinWait.SpinUntil(() => timer.Elapsed >= this._timeWindow);
                }
            });
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
            //这种方式控制流量,容易出现在某一个时刻,流量特别大,把 timeWindow 时间内的额度用光,导致后面一段时间内的请求都被拒绝了.
            if (amount + this._counter > this.RateLimit * this._timeWindow.TotalSeconds) return false;

            this._counter += amount;
            Task.Run(exec);
            return true;
        }
    }
}
