using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService.ConfigureCreater
{
    public class MySqlConfigureCreater :BaseConfigureCreater
    {
        public MySqlConfigureCreater(ConnectionInfo info) : base(info)
        {
        }

        protected override string FileName { get; set; } = "mysql.txt";
    }
}
