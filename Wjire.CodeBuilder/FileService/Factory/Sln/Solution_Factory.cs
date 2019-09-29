using System;
using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Solution_Factory : BaseFactory
    {
        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib\\SolutionTemplate\\Solution.txt");
        }

        protected override string ReplaceTemplateOfLine(string line, FormInfo formInfo)
        {
            return line
                    .Replace(TemplatePlaceholder.NameSpaceName, formInfo.NameSpaceName)
                    .Replace(TemplatePlaceholder.IRepositoryGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.IServiceGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.LogicGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.ModelGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.ProjectGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.RepositoryGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.ServiceGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.SolutionGuid, Guid.NewGuid().ToString())
                    .Replace(TemplatePlaceholder.DbContextGuid, Guid.NewGuid().ToString())
                ;
        }

        protected override string GetToSavePath(FormInfo formInfo)
        {
            return Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.sln");
        }

        protected override bool IsCover()
        {
            return false;
        }
    }
}
