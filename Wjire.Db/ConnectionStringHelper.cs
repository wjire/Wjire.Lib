using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;

namespace Wjire.Db
{

    /// <summary>
    /// 连接字符串操作类
    /// </summary>
    public static class ConnectionStringHelper
    {

        private static readonly IConfigurationSection Section;

        static ConnectionStringHelper()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(baseDirectory);
            string path = Path.Combine(baseDirectory, "appsettings.Development.json");
            IConfigurationRoot config;
            if (File.Exists(path))
            {
                config = builder.AddJsonFile("appsettings.Development.json", false, true).Build();
            }
            else
            {
                config = builder.AddJsonFile("appsettings.json", false, true).Build();
            }
            Section = config.GetSection("connectionStrings");
        }


        /// <summary>
        /// 读取配置文件,获取连接字符串
        /// </summary>
        /// <returns></returns>
        public static ConnectionStringSettings GetConnectionStringSettings(string name)
        {
            return Section.GetSection(name).Get<ConnectionStringSettings>();
        }
    }
}
