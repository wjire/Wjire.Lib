using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wjire.Core.Test
{
    public class Waiter<T>
    {
        private SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(0, 1);
        private DateTime _start;
        private DateTime _end;
        public Task<T> Task { get; set; }
        public TimeSpan Ts => _end - _start;

        public async Task<bool> WaitAsync(TimeSpan timeout)
        {
            return await this.Semaphore.WaitAsync(timeout);
        }

        public void AddTask(Func<Task<T>> func)
        {
            _start = DateTime.Now;
            var task = func();
            task.ContinueWith(t =>
            {
                _end = DateTime.Now;
                this.Semaphore.Release();
            }).ConfigureAwait(false);
            this.Task = task;
        }
    }
}
