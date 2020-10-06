using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSRedis;

namespace Wjire.Test
{
    public class Sample1
    {
        public static void Test()
        {
            CSRedisClient redisClient = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0");
            var lockKey = "lockKey";
            var stock = 5;//商品库存
            var taskCount = 10;//线程数量
            var script = "if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";//释放锁的redis脚本

            redisClient.Del(lockKey);//测试前,先把锁删了.

            for (int i = 0; i < taskCount; i++)
            {
                Task.Run(() =>
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

                    if (stock <= 0)
                    {
                        Console.WriteLine($"库存不足,线程:{Task.CurrentId} 抢购失败!");
                        redisClient.Del(lockKey);
                        return;
                    }

                    stock--;
                    //模拟处理业务,这里不考虑失败的情况
                    Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 3)));

                    Console.WriteLine($"线程:{Task.CurrentId} 消费完毕!剩余 {stock} 个");

                    //业务处理完后,释放锁.
                    redisClient.Eval(script, lockKey, id);
                });
            }
        }
    }
}
