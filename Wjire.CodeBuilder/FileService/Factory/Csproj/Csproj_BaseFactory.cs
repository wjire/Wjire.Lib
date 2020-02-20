using System;
using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public abstract class CsprojAbstractFactory : AbstractFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib\\CsprojTemplate");
        }

        protected override string ReplaceTemplateOfLine(string line, FormInfo formInfo)
        {
            return line.Replace(TemplatePlaceholder.NameSpaceName, formInfo.NameSpaceName);
        }

        protected override bool IsCover()
        {
            return false;
        }
    }
}
