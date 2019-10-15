using Microsoft.Web.Administration;
using System.Collections.Generic;
using Wjire.ProjectManager.WebApi.Model;

namespace Wjire.ProjectManager.WebApi.Service
{
    public class PublishService
    {


        /// <summary>
        /// 获取所有IIS应用程序名称
        /// </summary>
        /// <param name="applicationHostPath"></param>
        /// <returns></returns>
        public IEnumerable<AppInfoView> GetAllIISAppInfo(string applicationHostPath)
        {
            List<AppInfoView> result = new List<AppInfoView>();
            using (ServerManager iisManager = new ServerManager(applicationHostPath))
            //using (ServerManager iisManager = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
            {
                foreach (Site site in iisManager.Sites)
                {
                    AppInfoView app = new AppInfoView
                    {
                        Id = site.Id,
                        Name = site.Name,
                        Path = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                        Status = site.State == ObjectState.Started ? 1 : 0
                    };
                    result.Add(app);
                }
            }
            return result;
        }


        public bool Publish()
        {
            return true;
        }
    }
}
