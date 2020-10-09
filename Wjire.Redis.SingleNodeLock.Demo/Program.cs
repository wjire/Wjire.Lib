using System;
using System.Threading;
using System.Threading.Tasks;
using CSRedis;

namespace Wjire.Redis.SingleNodeLock.Demo
{
    /// <summary>
    /// Redis单节点分布式锁
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CSRedisClient redisClient = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0");
            var lockKey = "lockKey";
            var stockKey = "stock";
            redisClient.Set(stockKey, 5);//商品库存
            var releaseLockScript = "if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";//释放锁的redis脚本

            redisClient.Del(lockKey);//测试前,先把锁删了.

            Parallel.For(0, 10, i =>
            {
                var id = Guid.NewGuid().ToString("N");
                //获取锁
                do
                {
                    //set : key存在则失败,不存在才会成功,并且过期时间5秒
                    var success = redisClient.Set(lockKey, id, expireSeconds: 5, exists: RedisExistence.Nx);
                    if (success == true)
                    {
                        break;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(1));//休息1秒再尝试获取锁
                } while (true);

                Console.WriteLine($"线程:{Task.CurrentId} 拿到了锁,开始消费");

                //扣减库存
                var currentStock = redisClient.IncrBy(stockKey, -1);
                if (currentStock < 0)
                {
                    Console.WriteLine($"库存不足,线程:{Task.CurrentId} 抢购失败!");
                    redisClient.Eval(releaseLockScript, lockKey, id);
                    return;
                }

                //模拟处理业务,这里不考虑失败的情况
                Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 3)));

                Console.WriteLine($"线程:{Task.CurrentId} 消费完毕!剩余 {currentStock} 个");

                //业务处理完后,释放锁.
                redisClient.Eval(releaseLockScript, lockKey, id);
            });
        }
    }
}
