using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Wjire.Log;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Utils;

namespace Wjire.ProjectManager.WebApi.Service
{


    public class IISPublishService : BasePublishService
    {
        private readonly string _applicationHostPath;

        public IISPublishService()
        {

        }


        public IISPublishService(AppInfo appInfo) : base(appInfo)
        {
            _applicationHostPath = ConfigurationHelper.GetConfiguration("applicationHostPath");
        }


        /// <summary>
        /// 获取所有IIS应用程序信息
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AppInfo> GetAppInfos()
        {
            List<AppInfo> result = new List<AppInfo>();

            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                foreach (Site site in iisManager.Sites)
                {
                    AppInfo app = new AppInfo
                    {
                        AppId = site.Id,
                        AppName = site.Name,
                        AppPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                        Status = site.State == ObjectState.Started ? 1 : 0,
                    };
                    result.Add(app);
                }
            }
            return result;
        }


        protected override void StopApp()
        {
            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                Site site = iisManager.Sites.FirstOrDefault(f => f.Id == AppInfo.AppId);
                if (site == null)
                {
                    throw new ArgumentException($"未找到{AppInfo.AppId}的网站");
                }
                ApplicationPool appPool = iisManager.ApplicationPools[site.Name];
                if (appPool == null)
                {
                    throw new Exception($"{site.Name}应用程序池不存在");
                }                
                if (site.State != ObjectState.Stopped)
                {
                    site.Stop();
                }

                if (appPool.State != ObjectState.Stopped)
                {
                    appPool.Stop();
                }
                iisManager.CommitChanges();
                while (site.State != ObjectState.Stopped || appPool.State != ObjectState.Stopped)
                {
                    Thread.Sleep(500);
                }
            }
        }

        protected override string GetNewPath()
        {
            AppInfo app = GetAppInfo(AppInfo.AppId);
            string[] arr = app.AppPath.Split(".");
            int number = Convert.ToInt32(arr[arr.Length - 1]);
            number += 1;
            arr[arr.Length - 1] = number.ToString();
            string newPath = string.Join(".", arr);
            return newPath;
        }

        protected override string GetCurrentPath()
        {
            AppInfo app = GetAppInfo(AppInfo.AppId);
            return app.AppPath;
        }


        /// <summary>
        /// 获取IIS应用程序信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private AppInfo GetAppInfo(long id)
        {
            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                Site site = iisManager.Sites.FirstOrDefault(f => f.Id == id);
                if (site == null)
                {
                    throw new ArgumentException($"未找到{id}的网站");
                }
                AppInfo app = new AppInfo
                {
                    AppId = site.Id,
                    AppName = site.Name,
                    AppPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                    Status = site.State == ObjectState.Started ? 1 : 0
                };
                return app;
            }

        }


        protected override void CreateNewVersion(string newPath)
        {
            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                Site site = iisManager.Sites.FirstOrDefault(f => f.Id == AppInfo.AppId);
                if (site == null)
                {
                    throw new ArgumentException($"未找到{AppInfo.AppId}的网站");
                }
                site.Applications["/"].VirtualDirectories["/"].PhysicalPath = newPath;

                iisManager.CommitChanges();

                ApplicationPool appPool = iisManager.ApplicationPools[site.Name];
                if (appPool == null)
                {
                    throw new Exception($"{site.Name}应用程序池不存在");
                }

                if (appPool.State != ObjectState.Started)
                {
                    appPool.Start();
                }
                else
                {
                    appPool.Recycle();
                }
                iisManager.CommitChanges();
            }
        }


        protected override void CoverCurrentVersion(string path)
        {
            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                Site site = iisManager.Sites.FirstOrDefault(f => f.Id == AppInfo.AppId);
                if (site == null)
                {
                    throw new ArgumentException($"未找到{AppInfo.AppId}的网站");
                }
                if (site.State != ObjectState.Started)
                {
                    site.Start();
                }
                ApplicationPool appPool = iisManager.ApplicationPools[site.Name];
                if (appPool == null)
                {
                    throw new Exception($"{site.Name}应用程序池不存在");
                }
                if (appPool.State != ObjectState.Started)
                {
                    appPool.Start();
                }
                else
                {
                    appPool.Recycle();
                }
                iisManager.CommitChanges();
            }
        }
    }
}
