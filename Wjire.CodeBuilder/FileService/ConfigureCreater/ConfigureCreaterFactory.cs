using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService.ConfigureCreater
{
    public static class ConfigureCreaterFactory
    {
        public static BaseConfigureCreater Create(ConnectionInfo info)
        {
            switch (info.Type)
            {
                case "sqlserver":
                    return new SqlServerConfigureCreater(info);
                case "mysql":
                    return new MySqlConfigureCreater(info);
                default:
                    throw new Exception("尚不支持 " + info.Type);
            }
        }
    }
}
