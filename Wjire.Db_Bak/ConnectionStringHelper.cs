using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Wjire.Db
{

    /// <summary>
    /// 连接字符串操作类
    /// </summary>
    public static class ConnectionStringHelper
    {
        private const string Development = "appsettings.Development.json";
        private const string Release = "appsettings.json";
        private const string ConnectionStrings = "connectionStrings";

        private static readonly IConfigurationSection Section;

        static ConnectionStringHelper()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(baseDirectory);
            string path = Path.Combine(baseDirectory, Development);
            IConfigurationRoot config;
            if (File.Exists(path))
            {
                config = builder.AddJsonFile(Development, false, true).Build();
            }
            else
            {
                config = builder.AddJsonFile(Release, false, true).Build();
            }
            Section = config.GetSection(ConnectionStrings);
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
