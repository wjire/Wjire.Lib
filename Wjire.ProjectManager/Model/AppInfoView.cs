namespace Wjire.ProjectManager.Model
{
    public class AppInfoView
    {

        /// <summary>
        /// 程序ID
        /// </summary>
        public long AppId { get; set; }


        /// <summary>
        /// 项目名称
        /// </summary>
        public string AppName { get; set; }
        

        ///// <summary>
        ///// 服务器地址
        ///// </summary>
        //public string AppPath { get; set; }



        /// <summary>
        /// 本地地址
        /// </summary>
        public string LocalPath { get; set; }


        ///// <summary>
        ///// 1:IIS 2:EXE
        ///// </summary>
        //public int AppType { get; set; }


        public string AppTypeString { get; set; }

    }
}
