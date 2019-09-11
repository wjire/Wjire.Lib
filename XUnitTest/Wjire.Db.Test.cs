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
            System.Collections.Generic.List<ASORMInitLog> result = repo.GetList();
            ASORMInitLog model = repo.GetModel(2);
            int res = repo.Add(new ASORMInitLog { AppID = 3 }, "ASORMInitLog");
            Assert.True(result.Count > 0);
            Assert.True(model.ID == 2);
            Assert.True(res == 1);
        }
    }
}
