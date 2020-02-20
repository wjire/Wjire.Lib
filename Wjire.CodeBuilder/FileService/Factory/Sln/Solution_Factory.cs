using System;
using System.IO;
using Wjire.CodeBuilder.Model;

namespace Wjire.CodeBuilder.FileService
{

    public class Solution_Factory : AbstractFactory
    {
        private string FolderGuid = Guid.NewGuid().ToString();
        private string FolderEntityGuid = Guid.NewGuid().ToString();
        private string FolderLogicGuid = Guid.NewGuid().ToString();
        private string FolderRepositoryGuid = Guid.NewGuid().ToString();
        private string FolderWebApiGuid = Guid.NewGuid().ToString();

        private string IRepositoryGuid = Guid.NewGuid().ToString();
        private string IServiceGuid = Guid.NewGuid().ToString();
        private string LogicGuid = Guid.NewGuid().ToString();
        private string EntityGuid = Guid.NewGuid().ToString();
        private string DTOGuid = Guid.NewGuid().ToString();
        private string ProjectGuid = Guid.NewGuid().ToString();
        private string RepositoryGuid = Guid.NewGuid().ToString();
        private string ServiceGuid = Guid.NewGuid().ToString();
        private string SolutionGuid = Guid.NewGuid().ToString();
        private string DbContextGuid = Guid.NewGuid().ToString();
        private string WebApiGuid = Guid.NewGuid().ToString();

        protected override string GetFromTemplateInfoPath(FormInfo formInfo)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib\\SolutionTemplate\\Solution.txt");
        }

        protected override string ReplaceTemplateOfLine(string line, FormInfo formInfo)
        {
            return line
                    .Replace(TemplatePlaceholder.NameSpaceName, formInfo.NameSpaceName)
                    .Replace(TemplatePlaceholder.IRepositoryGuid, IRepositoryGuid)
                    .Replace(TemplatePlaceholder.IServiceGuid, IServiceGuid)
                    .Replace(TemplatePlaceholder.LogicGuid, LogicGuid)
                    .Replace(TemplatePlaceholder.EntityGuid, EntityGuid)
                    .Replace(TemplatePlaceholder.DTOGuid, DTOGuid)
                    .Replace(TemplatePlaceholder.ProjectGuid, ProjectGuid)
                    .Replace(TemplatePlaceholder.RepositoryGuid, RepositoryGuid)
                    .Replace(TemplatePlaceholder.ServiceGuid, ServiceGuid)
                    .Replace(TemplatePlaceholder.SolutionGuid, SolutionGuid)
                    .Replace(TemplatePlaceholder.DbContextGuid, DbContextGuid)
                    .Replace(TemplatePlaceholder.WebApiGuid, WebApiGuid)

                    //.Replace(TemplatePlaceholder.FolderGuid, FolderGuid)
                    //.Replace(TemplatePlaceholder.FolderEntityGuid, FolderEntityGuid)
                    //.Replace(TemplatePlaceholder.FolderLogicGuid, FolderLogicGuid)
                    //.Replace(TemplatePlaceholder.FolderRepositoryGuid, FolderRepositoryGuid)
                    //.Replace(TemplatePlaceholder.FolderWebApiGuid, FolderWebApiGuid)
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
