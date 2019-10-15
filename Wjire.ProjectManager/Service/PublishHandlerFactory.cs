namespace Wjire.ProjectManager.Service
{
    public static class PublishHandlerFactory
    {

        public static IPublishHandler Create(PublishInfo info)
        {
            switch (info.ProjectInfo.ProjectType)
            {
                case 1:
                    return new WebPublishHandler(info);
                case 2:
                    return new ExePublishHandler(info);
                default:
                    throw new System.Exception("项目类型异常");
            }
        }
    }
}
