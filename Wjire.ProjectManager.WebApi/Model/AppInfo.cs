namespace Wjire.ProjectManager.WebApi.Model
{
    public class AppInfo
    {
        public long AppId { get; set; }

        public string AppName { get; set; }

        public string AppPath { get; set; }

        public int AppType { get; set; }

        //0:关闭 1：开启
        public int Status { get; set; }
    }
}
