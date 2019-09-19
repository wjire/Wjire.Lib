using System;
using System.Collections.Generic;
using System.Diagnostics;
using wjire;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int count = 1;
            using (IASORMInitLogRepository repo = DbFactory.CreateIASORMInitLogRepositoryWrite())
            {
                {
                    ASORMInitLog log = new ASORMInitLog { AppID = 12, AppName = "wjire_test", CreatedAt = DateTime.Now.AddDays(100) };
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for (int i = 0; i < count; i++)
                    {
                        List<ASORMInitLog> list = repo.GetList("select * from ASORMInitLog");
                        foreach (ASORMInitLog item in list)
                        {
                            Console.WriteLine(item.AppName);
                        }
                    }

                    sw.Stop();
                    Console.WriteLine("测试一耗时 :" + sw.ElapsedMilliseconds + " ms");
                }
            }
            Console.ReadKey();
        }
    }
}
