using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Wjire.Db
{

    /// <summary>
    /// 连接字符串操作类
    /// </summary>
    public static class ConnectionStringHelper
    {

        private static readonly IConfigurationSection Section;

        private static readonly ConcurrentDictionary<string, ConnectionStringSettings> ConnectionStringInfoCache =
            new ConcurrentDictionary<string, ConnectionStringSettings>();

        static ConnectionStringHelper()
        {
            IConfigurationRoot config = null;
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var builder = new ConfigurationBuilder().SetBasePath(baseDirectory);
            string path = Path.Combine(baseDirectory, "appsettings.Development.json");
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
            return ConnectionStringInfoCache.GetOrAdd(name, key => Section.GetSection(key).Get<ConnectionStringSettings>());
        }
    }
}
