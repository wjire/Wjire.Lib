using System;
using System.Threading;
using System.Threading.Tasks;
using CSRedis;

namespace Wjire.Test
{
    class Program
    {
        static string key = "lock";
        private static string store = "store";
        static CSRedisClient redis = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0");

        static Program()
        {
            redis.Set(store, 5);
        }

        static void Main(string[] args)
        {
            //Sample1.Test();


            var lua = "local value=redis.call('set',KEYS[1],ARGV[1]);return value";
            var sha = redis.ScriptLoad(lua);//f6b1e488bfde42af6f14ee6c7e476ef0e386b907
            //sha = "f6b1e488bfde42af6f14ee6c7e476ef0e386b907";
            var result = redis.EvalSHA(sha, "test", 1);
            Console.WriteLine(result);
            lua = "local value=redis.call('get',KEYS[1]);value = value + 1;return value";
            sha = redis.ScriptLoad(lua);
            //sha = "64f1102d5839143f1686190dc0311eca7df2fd1c";
            result = redis.EvalSHA(sha, "test");
            Console.WriteLine(result);
            Console.ReadKey();

            var obj = redis.IncrBy(store, -1);
            Console.WriteLine(obj);
            Console.ReadKey();

            var lua1 = "local value = redis.call('get',KEYS[1]);if value > ARGV[1] then redis.call('set',KEYS[1],value-1) end;return value";
            var expire = 20;
            while (true)
            {
                var count = redis.Get<int>(store);
                if (count <= 0)
                {
                    break;
                }
                Task.Run(() =>
                {
                    var id = Guid.NewGuid().ToString("N");
                    while ((redis.Set(key, id, expire, RedisExistence.Nx)) == false)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    Console.WriteLine("消费者: " + id + " 拿到了锁,开始消费");
                    try
                    {
                        //var obj = redis.Eval(lua1, store, 0);
                        var obj = redis.IncrBy(store, -1);
                        var currentStore = Convert.ToInt32(obj);
                        if (currentStore < 0)
                        {
                            Console.WriteLine("库存不足");
                            ReleaseLock(id);
                            return;
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 3)));
                        Console.WriteLine("消费者: " + id + $" 消费完毕,剩余 {currentStore} 个");
                        ReleaseLock(id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                });
                Thread.Sleep(500);
            }

            Console.WriteLine("卖完了");
            Console.ReadLine();
        }

        static void ReleaseLock(string id)
        {
            var script = "if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";
            try
            {
                var r = (long)redis.Eval(script, key, id);
                Console.WriteLine(id + " 释放锁:" + (r > 0 ? "成功" : "失败"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        static async Task Test()
        {
            await Task.CompletedTask;
            await Task.Delay(TimeSpan.FromSeconds(2));
            Console.WriteLine("in test()");
            //await Task.CompletedTask;
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("return test");
            }
        }
    }
}
