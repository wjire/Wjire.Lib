using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Utils;

namespace Wjire.ProjectManager.WebApi.Service
{
    public class WindowsServicePublishService : BasePublishService
    {

        private readonly string _rpcServicePath = ConfigurationHelper.GetConfiguration("rpcServicePath");

        public WindowsServicePublishService()
        {

        }

        public WindowsServicePublishService(AppInfo appInfo) : base(appInfo)
        {

        }



        /// <summary>
        /// 获取所有rpc服务
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AppInfo> GetAppInfos()
        {
            List<AppInfo> appInfos = new List<AppInfo>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service ");
            foreach (ManagementBaseObject mo in searcher.Get())
            {
                string pathName = mo["PathName"]?.ToString();
                if (string.IsNullOrWhiteSpace(pathName) == true)
                {
                    continue;
                }
                if (mo["PathName"].ToString().Contains(_rpcServicePath) == false)
                {
                    continue;
                }
                appInfos.Add(new AppInfo
                {
                    AppId = Convert.ToInt64(mo["ProcessId"]),
                    AppPath = Path.GetDirectoryName(pathName),
                    AppName = mo["Name"].ToString(),
                    Status = mo["Started"].Equals(true) ? 1 : 0,
                    AppType = 2,
                });
            }
            return appInfos;
        }


        ///// <summary>
        ///// 获取所有rpc服务
        ///// </summary>
        ///// <returns></returns>
        //public override IEnumerable<AppInfo> GetAppInfos2()
        //{
        //    string[] dirs = Directory.GetDirectories(_rpcServicePath);
        //    return dirs.Select(s => new AppInfo
        //    {
        //        AppName = Path.GetFileName(s),
        //        AppType = 2,
        //    });
        //}


        protected override void StopApp()
        {
            AppInfo app = GetAppInfo(AppInfo.AppName);
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

                proc.StartInfo.WorkingDirectory = app.AppPath;

                proc.Start();

                //构造命令
                string command = GetStopCommand(app.AppName);

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(command);
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


        protected override string GetNewPath()
        {
            string path = Path.GetDirectoryName(GetAppInfo(AppInfo.AppName).AppPath);
            string[] dirs = Directory.GetDirectories(path);
            List<Version> versions = dirs.Select(Path.GetFileName).Select(versionString => new Version(versionString)).ToList();
            Version max = versions.Max();
            string[] arr = max.ToString().Split(".");
            int lastNumber = Convert.ToInt32(arr[arr.Length - 1]);
            arr[arr.Length - 1] = (++lastNumber).ToString();
            string newVersionString = string.Join(".", arr);
            return Path.Combine(path, newVersionString);
        }

        //protected override string GetNewPath()
        //{
        //    string path = Path.Combine(_rpcServicePath, AppInfo.AppName);
        //    string[] dirs = Directory.GetDirectories(path);
        //    List<Version> versions = dirs.Select(Path.GetFileName).Select(versionString => new Version(versionString)).ToList();
        //    Version max = versions.Max();
        //    string[] arr = max.ToString().Split(".");
        //    int lastNumber = Convert.ToInt32(arr[arr.Length - 1]);
        //    arr[arr.Length - 1] = (++lastNumber).ToString();
        //    string newVersionString = string.Join(".", arr);
        //    return Path.Combine(path, newVersionString);
        //}


        protected override string GetCurrentPath()
        {
            return GetAppInfo(AppInfo.AppName).AppPath;
        }



        private AppInfo GetAppInfo(string name)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Service where Name='{name}' ");

            foreach (ManagementBaseObject mo in searcher.Get())
            {

                return new AppInfo
                {
                    AppPath = Path.GetDirectoryName(mo["PathName"]?.ToString()),
                    AppName = mo["Name"].ToString(),

                };
            }

            throw new Exception($"未找到{name}的服务");
        }



        protected override void CreateNewVersion(string newPath)
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
                string command = GetStartCommand(newPath);

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(command);
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

        protected override void CoverCurrentVersion(string path)
        {
            AppInfo app = GetAppInfo(AppInfo.AppName);
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

                proc.StartInfo.WorkingDirectory = path;

                proc.Start();

                //构造命令
                string command = GetReStartCommand(app.AppName);

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(command);
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


        private string GetStartCommand(string path)
        {
            /*
             *@echo off
                title Microservice.EventTracking
                net stop Microservice.EventTracking
                sc delete Microservice.EventTracking
                sc create Microservice.EventTracking binpath= "%~dp0%..\ConsoleApp" displayname= "Microservice.EventTracking" depend= Tcpip start= auto
                net start Microservice.EventTracking
                exit
             *
             *
             */

            StringBuilder sb = new StringBuilder();
            sb.Append($"net stop {AppInfo.AppName}");
            sb.Append($"&sc delete {AppInfo.AppName}");
            sb.Append($"&sc create {AppInfo.AppName} binpath= \"{GetWindowsServiceBindPath(path)}\" displayname= \"{AppInfo.AppName}\" depend= Tcpip start= auto");
            sb.Append($@"&net start {AppInfo.AppName}");
            sb.Append("&exit");
            return sb.ToString();
        }


        private string GetReStartCommand(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"&net start {name}");
            sb.Append("&exit");
            return sb.ToString();
        }


        private string GetStopCommand(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"net stop {name}");
            sb.Append("&exit");
            return sb.ToString();
        }


        private string GetWindowsServiceBindPath(string path)
        {
            string[] files = Directory.GetFiles(path, "*.exe");
            if (files.Count() == 1)
            {
                return Path.Combine(path, Path.GetFileNameWithoutExtension(files[0]));
            }

            throw new Exception($"未找到 .exe 文件");
        }
    }
}
