using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Wjire.Core.Test
{
    public class WaiterAsyncManual<T>
    {
        private AsyncManualResetEvent Semaphore { get; } = new AsyncManualResetEvent();
        private DateTime _start;
        private DateTime _end;
        public Task<T> Task { get; set; }
        public TimeSpan Ts => _end - _start;

        public async Task WaitAsync(CancellationToken token)
        {
            await this.Semaphore.WaitAsync(token);
        }

        public void AddTask(Func<Task<T>> func)
        {
            _start = DateTime.Now;
            var task = func();
            task.ContinueWith(t =>
            {
                _end = DateTime.Now;
            }).ConfigureAwait(false);
            this.Task = task;
        }
    }
}
