﻿using System;
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
                    AppPath = pathName,
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
            throw new NotImplementedException();
        }


        protected override string GetNewPath()
        {
            string path = Path.Combine(_rpcServicePath, AppInfo.AppName);
            string[] dirs = Directory.GetDirectories(path);
            List<Version> versions = dirs.Select(Path.GetFileName).Select(versionString => new Version(versionString)).ToList();
            Version max = versions.Max();
            string[] arr = max.ToString().Split(".");
            int lastNumber = Convert.ToInt32(arr[arr.Length - 1]);
            arr[arr.Length - 1] = (++lastNumber).ToString();
            string newVersionString = string.Join(".", arr);
            return Path.Combine(path, newVersionString);
        }


        protected override string GetCurrentPath()
        {
            string path = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Service where Name = '{AppInfo.AppName}'"))
            {
                ManagementObjectCollection collection = searcher.Get();
                int count = collection.Count;
                if (count != 1)
                {
                    throw new Exception($"找到{count}个名为{AppInfo.AppName}的服务");
                }
                foreach (ManagementBaseObject mo in collection)
                {
                    path = mo["PathName"].ToString();
                }
            }
            return path;
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
                string command = GetCommand(newPath);

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
            throw new NotImplementedException();
        }


        private string GetCommand(string path)
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
