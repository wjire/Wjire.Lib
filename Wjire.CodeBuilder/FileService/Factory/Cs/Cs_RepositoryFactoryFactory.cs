using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Cs_RepositoryFactoryFactory : Cs_BaseFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(base.GetFromTemplateInfoPath(formInfo), "RepositoryFactory.txt");
        }

        protected override string GetToSavePath(FormInfo formInfo)
        {
            return Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.DbContext\\{formInfo.TableName}RepositoryFactory.cs");
        }
    }
}
