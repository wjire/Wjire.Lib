using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.DbService
{
    public abstract class BaseDbService
    {
        public ConnectionInfo ConnectionInfo { get; set; }
        protected string ConnectionString { get; set; }

        protected BaseDbService(ConnectionInfo info)
        {
            ConnectionInfo = info;
        }
    }
}
