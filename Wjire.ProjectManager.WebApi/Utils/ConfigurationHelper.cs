using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Wjire.ProjectManager.WebApi.Utils
{
    public class ConfigurationHelper
    {

        private static readonly IConfigurationRoot config;

        static ConfigurationHelper()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(baseDirectory);
            config = builder.AddJsonFile("appsettings.json", false, true).Build();
        }


        public static string GetConfiguration(string name)
        {
            return config[name];
        }
    }
}
