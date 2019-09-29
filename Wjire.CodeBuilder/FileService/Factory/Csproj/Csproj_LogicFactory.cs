using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Csproj_LogicFactory : Csproj_BaseFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(base.GetFromTemplateInfoPath(formInfo), "Logic.txt");
        }

        protected override string GetToSavePath(FormInfo formInfo)
        {
            return Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.Logic\\{formInfo.NameSpaceName}.Logic.csproj");
        }
    }
}
