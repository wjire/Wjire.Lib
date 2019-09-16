using System.Collections.Generic;
using Wjire.Db;
using Xunit;

namespace XUnitTest
{
    public class WjireDbTest
    {
        [Fact]
        public void ConfigureHelperTest()
        {
            string db1 = ConnectionStringHelper.GetConnectionString("db1");
            string db2 = ConnectionStringHelper.GetConnectionString("db2");
            Assert.True(db1 == "1");
            Assert.True(db2 == "2");
        }

        [Fact]
        public void ConnectionTest()
        {
            ASORMInitLogRepository repo = new ASORMInitLogRepository("MagicTaskRecordRead");
            List<ASORMInitLog> result = repo.GetList();
            //ASORMInitLog model = repo.GetSingle(0,"es");
            //int res = repo.Add(new ASORMInitLog { AppID = 3 }, "ASORMInitLog");
            //Assert.True(result.Count > 0);
            //Assert.True(model.AppName == "test");
            //Assert.True(res == 1);
        }



        [Fact]
        public void AddTest()
        {
            ASORMInitLogRepository repo = new ASORMInitLogRepository("MagicTaskRecordRead");
            ASORMInitLog log = new ASORMInitLog() { AppName = "2234234234" };
            int i = repo.Add(log);
            Assert.True(i > 0);
        }
    }
}
