using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace ConsoleApp1
{
    public class Waiter<T> : IDisposable
    {
        private AsyncManualResetEvent Semaphore { get; } = new AsyncManualResetEvent();
        private DateTime _start;
        private DateTime _end;
        public Task<T> Task { get; set; }
        public TimeSpan Ts => _end - _start;

        public async Task WaitAsync(TimeSpan timeout)
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);
            await WaitAsync(cts.Token);
        }

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
                Semaphore.Set();
            }).ConfigureAwait(false);
            this.Task = task;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Task?.Dispose();
        }
    }
}
