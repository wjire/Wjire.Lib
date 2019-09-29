using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Csproj_RepositoryFactory : Csproj_BaseFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(base.GetFromTemplateInfoPath(formInfo), "IRepository.txt");
        }

        protected override string GetToSavePath(FormInfo formInfo)
        {
            return Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.Repository\\{formInfo.NameSpaceName}.Repository.csproj");
        }
    }
}
