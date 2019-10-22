using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Wjire.Log
{
    /// <summary>
    /// appsettings.json 配置文件操作类
    /// </summary>
    public static class ConfigureHelper
    {

        /// <summary>
        /// Config
        /// </summary>
        public static readonly IConfigurationRoot Config;

        static ConfigureHelper()
        {
            if (Config != null)
            {
                return;
            }

            ConfigurationBuilder builder = new ConfigurationBuilder();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.Development.json");
            if (File.Exists(path) == false)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            }
            builder.AddJsonFile(path, false, true);
            Config = builder.Build();
        }


        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns>string</returns>
        public static string GetString(string key)
        {
            try
            {
                return Config[key];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
