using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using Wjire.ProjectManager.Model;

namespace Wjire.ProjectManager
{
    public class PublishHandler
    {
        protected readonly PublishInfo Info;
        private readonly string _uploadApi = System.Configuration.ConfigurationManager.AppSettings["uploadApi"];


        public PublishHandler()
        {

        }

        public PublishHandler(PublishInfo info)
        {
            Info = info;
        }


        public bool PublishApp()
        {
            try
            {
                bool res = PublishDll();
                if (res == false)
                {
                    return false;
                }
                Pack();
                Upload();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// 发布dll
        /// </summary>
        protected virtual bool PublishDll()
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

                proc.Start();

                //构造命令
                StringBuilder sb = new StringBuilder();

                sb.Append($@"dotnet publish {Info.AppInfo.LocalPath} -c release -o publish\{Info.AppInfo.AppName}");
                //退出
                sb.Append("&exit");

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(sb.ToString());
                proc.StandardInput.AutoFlush = true;

                output = proc.StandardOutput.ReadToEnd();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                proc.WaitForExit();
                proc.Close();
                Console.WriteLine(output);
            }
        }


        /// <summary>
        /// 压缩
        /// </summary>
        protected virtual void Pack()
        {
            FastZip fz = new FastZip { CreateEmptyDirectories = true };
            fz.CreateZip(Info.FileName, $@"publish\{Info.AppInfo.AppName}", true, Info.FileFilter);
            fz = null;
        }



        /// <summary>
        /// 上传
        /// </summary>
        protected virtual string Upload()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uploadApi);
                //client.BaseAddress = new Uri("http://localhost:52635");
                string apiUrl = "api/publish/upload";
                MultipartFormDataContent content = new MultipartFormDataContent();
                FileStream fs = new FileStream(Info.FileName, FileMode.Open, FileAccess.Read);
                content.Add(new StreamContent(fs), "file", Path.GetFileName(Info.FileName));
                content.Add(new StringContent(JsonConvert.SerializeObject(Info.AppInfo)), nameof(AppInfo));
                HttpResponseMessage result = client.PostAsync(apiUrl, content).Result;
                if (result.IsSuccessStatusCode == false)
                {
                    Console.WriteLine(result.Content);
                }
                fs.Dispose();
                return result.Content.ReadAsStringAsync().Result;
            }
        }



        public List<AppInfo> GetAppInfos(int appType)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uploadApi);
                string apiUrl = "api/publish/getappInfos?type=" + appType;
                string result = client.GetStringAsync(apiUrl).Result;
                return JsonConvert.DeserializeObject<List<AppInfo>>(result);
            }
        }
    }
}
