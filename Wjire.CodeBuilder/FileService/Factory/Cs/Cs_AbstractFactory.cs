using System;
using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public abstract class CsAbstractFactory : AbstractFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib\\CsTemplate");
        }

        protected override string ReplaceTemplateOfLine(string line, FormInfo formInfo)
        {
            return line
                    .Replace(TemplatePlaceholder.NameSpaceName, formInfo.NameSpaceName)
                    .Replace(TemplatePlaceholder.TableName, formInfo.TableName)
                    .Replace(TemplatePlaceholder.DbName, formInfo.DbName)
                ;
        }
    }
}
