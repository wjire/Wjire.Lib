using System;
using System.IO;
using System.Text;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService.ConfigureCreater
{
    public abstract class BaseConfigureCreater
    {
        private readonly ConnectionInfo _info;

        protected string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"Lib\ConnectionStringTemplate");

        protected BaseConfigureCreater(ConnectionInfo info)
        {
            _info = info;
        }

        public string CreateConfigure()
        {
            string[] lines = File.ReadAllLines(Path.Combine(FilePath, FileName));
            StringBuilder sb = new StringBuilder(512);
            foreach (string line in lines)
            {
                string item = line
                    .Replace(TemplatePlaceholder.DbName, _info.DbName)
                    .Replace(TemplatePlaceholder.Host, _info.IP)
                    .Replace(TemplatePlaceholder.Account, _info.User)
                    .Replace(TemplatePlaceholder.Pwd, _info.Pwd);
                sb.AppendLine(item);
            }
            string content = sb.ToString();
            return content;
        }

        protected abstract string FileName { get; set; }
    }
}
