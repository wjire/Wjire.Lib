using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dapper;
using Wjire.Excel;
using Wjire.Model;
using Xunit;

namespace Wjire.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string connectionString = "Server=localhost;Initial Catalog=MagicTaskRecord;User ID=sa;Password=Aa1111;";
            List<ASORMInitLog> datas = null;
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                datas = db.Query<ASORMInitLog>("SELECT * FROM ASORMInitLog").ToList();
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            byte[] bytes = null;

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds/1000);

            var path = @"C:\Users\Administrator\Desktop\1.xlsx";
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(bytes);
            fs.Dispose();
        }
    }
}
