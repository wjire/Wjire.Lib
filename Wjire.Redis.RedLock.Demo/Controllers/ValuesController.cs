using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedLockNet;

namespace Wjire.Redis.RedLock.Demo.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static int _stock = 10;

        private readonly IDistributedLockFactory _distributedLockFactory;

        public ValuesController(IDistributedLockFactory distributedLockFactory)
        {
            _distributedLockFactory = distributedLockFactory;
        }

        [Route("lockTest")]
        [HttpGet]
        public async Task<int> DistributedLockTest()
        {
            // resource 锁定的资源
            var resource = "the-thing-we-are-locking-on";

            // expiryTime 锁的过期时间
            var expiry = TimeSpan.FromSeconds(5);

            // waitTime 等待时间
            var wait = TimeSpan.FromSeconds(1);

            // retryTime 等待时间内，多久重试一次
            var retry = TimeSpan.FromMilliseconds(250);

            using (var redLock = await _distributedLockFactory.CreateLockAsync(resource, expiry, wait, retry))
            {
                if (redLock.IsAcquired)
                {
                    // 模拟执行业务逻辑
                    await Task.Delay(new Random().Next(100, 500));
                    if (_stock > 0)
                    {
                        _stock--;
                        return _stock;
                    }
                    return _stock;
                }
                Console.WriteLine($"{DateTime.Now} : 获取锁失败");
            }
            return -99;
        }
    }
}
