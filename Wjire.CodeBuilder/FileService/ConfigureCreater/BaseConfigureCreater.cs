using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var lines = File.ReadAllLines(Path.Combine(FilePath, FileName));
            StringBuilder sb = new StringBuilder(512);
            foreach (var line in lines)
            {
                var item = line
                    .Replace(TemplatePlaceholder.DbName, _info.DbName)
                    .Replace(TemplatePlaceholder.Host, _info.IP)
                    .Replace(TemplatePlaceholder.Account, _info.User)
                    .Replace(TemplatePlaceholder.Pwd, _info.Pwd);
                sb.AppendLine(item);
            }
            var content = sb.ToString();
            return content;
        }

        protected abstract string FileName { get; set; }
    }
}
