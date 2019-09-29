using wjire;
using Wjire.Db;

namespace ConsoleTest
{
    public class TestRepository : BaseRepository<ASORMInitLog>
    {
        public TestRepository(string name) : base(name) { }

        public TestRepository(IUnitOfWork unit) : base(unit) { }
    }
}
