using System;
using System.Diagnostics;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int count = 100000;

            using (ASORMInitLogRepository repo = new ASORMInitLogRepository("MagicTaskRecordRead"))
            {
                {
                    ASORMInitLog log = new ASORMInitLog { AppID = 2, ID = 3, AppName = "wjire" };
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for (int i = 0; i < count; i++)
                    {
                        repo.UpdateTest(log);
                    }

                    sw.Stop();
                    Console.WriteLine("测试一耗时 :" + sw.ElapsedMilliseconds + " ms");
                }
            }
        }
    }
}
