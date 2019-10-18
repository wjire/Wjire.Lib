using System;
using Wjire.ProjectManager.WebApi.Model;

namespace Wjire.ProjectManager.WebApi.Service
{
    public static class PublishServiceFactory
    {
        public static BasePublishService Create(AppInfo appInfo)
        {
            switch (appInfo.AppType)
            {
                case 1:
                    return new IISPublishService(appInfo);
                case 2:
                    return new ExePublishService(appInfo);
                default:
                    throw new ArgumentException(nameof(appInfo));
            }
        }


        public static BasePublishService Create(int type)
        {
            switch (type)
            {
                case 1:
                    return new IISPublishService();
                case 2:
                    return new ExePublishService();
                default:
                    throw new ArgumentException(nameof(type));
            }
        }
    }
}
