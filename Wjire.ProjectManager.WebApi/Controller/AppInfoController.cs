using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Service;

namespace Wjire.ProjectManager.WebApi.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class AppInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PublishService _publishService;

        public AppInfoController(IConfiguration configuration, PublishService publishService)
        {
            _configuration = configuration;
            _publishService = publishService;
        }
        

        public IEnumerable<AppInfoView> Get()
        {
            string applicationHostPath = _configuration["applicationHostPath"];
            return _publishService.GetAllIISAppInfo(applicationHostPath);
        }

        //public AppInfo GetAllIISAppInfo(long id)
        //{
        //    return new AppInfo();
        //}


        public static void CreateSite(string name, string protocol, string address, string path)
        {
            ServerManager iisManager = new ServerManager();
            //iisManager.Sites.Add("NewSite", "http", "*:9527:", @"H:\gongwei\fabu\Admin");
            iisManager.Sites.Add(name, protocol, address, path);
            iisManager.CommitChanges();
        }


        private static bool PublishWeb(string webName, int port)
        {
            try
            {
                ServerManager iisManager = new ServerManager();

                //判断应用程序池是否存在
                if (iisManager.ApplicationPools[webName] != null)
                {
                    iisManager.ApplicationPools.Remove(iisManager.ApplicationPools[webName]);
                }
                //判断web应用程序是否存在
                if (iisManager.Sites[webName] != null)
                {
                    iisManager.Sites.Remove(iisManager.Sites[webName]);
                }

                //建立web应用程序（第二个参数为安装文件的地址）
                iisManager.Sites.Add(webName, "c:\\webFilePath", port);
                //添加web应用程序池
                ApplicationPool pool = iisManager.ApplicationPools.Add(webName);
                //设置web应用程序池的Framework版本（注意版本号大小写问题）
                pool.ManagedRuntimeVersion = "v4.0";
                //设置是否启用32为应用程序
                pool.SetAttributeValue("enable32BitAppOnWin64", true);
                //设置web网站的应用程序池
                iisManager.Sites[webName].Applications[0].ApplicationPoolName = webName;
                //提交更改
                iisManager.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}