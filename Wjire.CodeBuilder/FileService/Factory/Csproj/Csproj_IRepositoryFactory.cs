using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Csproj_IRepositoryFactory : Csproj_BaseFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(base.GetFromTemplateInfoPath(formInfo), "IRepository.txt");
        }

        protected override string GetToSavePath(FormInfo formInfo)
        {
            return Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.IRepository\\{formInfo.NameSpaceName}.IRepository.csproj");
        }
    }
}
