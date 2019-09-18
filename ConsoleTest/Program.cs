using System;
using System.Diagnostics;

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
                    ASORMInitLog log = new ASORMInitLog { AppID = 12, AppName = "wjire_test" };
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for (int i = 0; i < count; i++)
                    {
                        ASORMInitLog model = repo.Query("select * from ASORMInitLog where id=@id", new { id = 9});
                        Console.WriteLine(model.AppName);
                    }

                    sw.Stop();
                    Console.WriteLine("测试一耗时 :" + sw.ElapsedMilliseconds + " ms");
                }
            }
        }
    }
}
