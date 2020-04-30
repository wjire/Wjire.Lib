using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Wjire.ASP.NET.Core3._1.Demo.Logics
{
    public class CacheTestLogic
    {
        private readonly IMemoryCache _cache;

        public CacheTestLogic(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Test()
        {
            _cache.GetOrCreate("key", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return "test";
            });
        }
    }
}
