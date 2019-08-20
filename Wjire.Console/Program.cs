using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Wjire.Common;
using Wjire.Excel;
using Wjire.Model;

namespace Wjire.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Initial Catalog=MagicTaskRecord;User ID=sa;Password=Aa1111;";
            List<ASORMInitLog> datas = null;
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                datas = db.Query<ASORMInitLog>("SELECT * FROM ASORMInitLog").ToList();
            }

            System.Console.WriteLine($"数据源对象 {typeof(ASORMInitLog).GetProperties().Length} 个字段,共 {datas.Count} 条记录,大小 {BinarySerializeHelper.SerializeToBytes(datas).Length / 1000 / 1000} M");
            Task.Run(() =>
            {
                while (true)
                {
                    System.Console.WriteLine($"{DateTime.Now} 内存 : {GC.GetTotalMemory(false) / 1000 / 1000} M");
                    Thread.Sleep(1000);
                }
            });

            Stopwatch sw = new Stopwatch();
            //System.Console.WriteLine($"{DateTime.Now} 1内存 : {GC.GetTotalMemory(false) / 1000 / 1000} M");

            //sw.Start();
            //System.Data.DataTable dt = datas.ToDataTable();
            //System.Console.WriteLine($"{DateTime.Now} 2内存 : {GC.GetTotalMemory(false) / 1000 / 1000} M");
            //sw.Stop();
            //System.Console.WriteLine("List 转换成 DataTable 耗时 :" +sw.ElapsedMilliseconds/1000 +" 秒");

            string path = @"C:\Users\Administrator\Desktop\123.xlsx";
            var set = new HashSet<string> {"ID"};
            sw.Start();
            //byte[] bytes = ExcelHandlerFactory.CreateHandler(datas).CreateExcelBytes();
            //byte[] bytes = EpPlusHelper.ExportByEPPlus(dt);
            //byte[] bytes = EpPlusHelper.ExportByCollection(datas);
            //byte[] bytes = ExcelHelper.CreateBytes(datas);
            ExcelHelper.CreateFile(datas,path);


            sw.Stop();
            System.Console.WriteLine($"{DateTime.Now} 导出完毕,共耗时 : " + sw.ElapsedMilliseconds / 1000 + " 秒");
            //FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            //fs.Write(bytes);
            //fs.Dispose();
            System.Console.ReadKey();
        }
    }
}
