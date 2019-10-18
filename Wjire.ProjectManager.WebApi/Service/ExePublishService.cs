using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Wjire.ProjectManager.WebApi.Model;

namespace Wjire.ProjectManager.WebApi.Service
{
    public class ExePublishService : BasePublishService
    {

        private readonly string _rpcServicePath;

        public ExePublishService()
        {
            
        }

        public ExePublishService(AppInfo appInfo) : base(appInfo)
        {
            _rpcServicePath = new ConfigurationBuilder().Build()["rpcServicePath"];
        }


        /// <summary>
        /// 获取所有rpc服务
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AppInfo> GetAppInfos()
        {
            List<AppInfo> result = new List<AppInfo>();
            return result;
        }



        protected override string GetNewPath()
        {
            return string.Empty;
        }


        protected override void ChangeAppVersion(string newPath)
        {

        }
    }
}
