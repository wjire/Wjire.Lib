using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Cs_AppsettingsFactory : Cs_WebApiAbstractFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(base.GetFromTemplateInfoPath(formInfo), "appsettings.Development.txt");
        }

        protected override string GetToSavePath(FormInfo formInfo)
        {
            return Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.WebApi\\appsettings.Development.json");
        }

        protected override string ReplaceTemplateOfLine(string line, FormInfo formInfo)
        {
            var str = base.ReplaceTemplateOfLine(line, formInfo);
            return str.Replace(TemplatePlaceholder.Pwd, formInfo.Pwd);
        }
    }
}
