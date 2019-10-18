using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Utils;

namespace Wjire.ProjectManager.WebApi.Service
{
    public class ExePublishService : BasePublishService
    {

        private readonly string _rpcServicePath = ConfigurationHelper.GetConfiguration("rpcServicePath");

        public ExePublishService()
        {

        }

        public ExePublishService(AppInfo appInfo) : base(appInfo)
        {

        }


        /// <summary>
        /// 获取所有rpc服务
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AppInfo> GetAppInfos()
        {
            string[] dirs = Directory.GetDirectories(_rpcServicePath);
            return dirs.Select(s => new AppInfo
            {
                AppName = Path.GetFileName(s),
                AppType = 2,
            });
        }



        protected override string GetNewPath()
        {
            string path = Path.Combine(_rpcServicePath, AppInfo.AppName);
            string[] dirs = Directory.GetDirectories(path);
            List<Version> versions = dirs.Select(item => Path.GetFileName(item)).Select(versionString => new Version(versionString)).ToList();
            Version max = versions.Max();
            string[] arr = max.ToString().Split(".");
            int lastNumber = Convert.ToInt32(arr[arr.Length - 1]);
            arr[arr.Length - 1] = (++lastNumber).ToString();
            string newVersionString = string.Join(".", arr);
            return Path.Combine(path, newVersionString);
        }



        protected override void ChangeAppVersion(string newPath)
        {
            Process proc = new Process();
            string output = null;
            try
            {
                proc.StartInfo.FileName = "cmd.exe";

                //是否使用操作系统shell启动
                proc.StartInfo.UseShellExecute = false;

                //接受来自调用程序的输入信息
                proc.StartInfo.RedirectStandardInput = true;

                //输出信息
                proc.StartInfo.RedirectStandardOutput = true;

                //输出错误
                proc.StartInfo.RedirectStandardError = true;

                //不显示程序窗口
                proc.StartInfo.CreateNoWindow = true;

                proc.StartInfo.WorkingDirectory = newPath;

                proc.Start();

                //构造命令
                StringBuilder sb = new StringBuilder();

                sb.Append($@"service.cmd");
                //退出
                sb.Append("&exit");

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(sb.ToString());
                proc.StandardInput.AutoFlush = true;

                output = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                proc.WaitForExit();
                proc.Close();
                Console.WriteLine(output);
            }
        }
    }
}
