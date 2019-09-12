using Microsoft.Extensions.Configuration;
using System;

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
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot config = builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json", false, true).Build();
            Section = config.GetSection("connectionStrings");
        }


        /// <summary>
        /// 读取配置文件,获取连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            return Section[name];
        }
    }
}
