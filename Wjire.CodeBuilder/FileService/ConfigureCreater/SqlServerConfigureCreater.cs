using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService.ConfigureCreater
{
    public class SqlServerConfigureCreater : BaseConfigureCreater
    {
        public SqlServerConfigureCreater(ConnectionInfo info) : base(info)
        {
        }

        protected override string FileName { get; set; } = "sqlserver.txt";
    }
}
