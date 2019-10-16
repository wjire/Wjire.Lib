using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Configuration;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wjire.ProjectManager.WebApi.Model;

namespace Wjire.ProjectManager.WebApi.Service
{
    public class PublishService
    {
        private readonly string _applicationHostPath;

        public PublishService(IConfiguration configuration)
        {
            _applicationHostPath = configuration["applicationHostPath"];
        }


        /// <summary>
        /// 获取所有IIS应用程序信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppInfo> GetAllIISAppInfo()
        {
            List<AppInfo> result = new List<AppInfo>();

            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                foreach (Site site in iisManager.Sites)
                {
                    AppInfo app = new AppInfo
                    {
                        AppId = site.Id,
                        AppName = site.Name,
                        AppPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                        Status = site.State == ObjectState.Started ? 1 : 0,
                    };
                    result.Add(app);
                }
            }
            return result;
        }


        /// <summary>
        /// 发布网站
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Publish(long id, Stream stream)
        {
            string newPath = GetNewPath(id);
            UnpackFiles(stream, newPath);
            ChangeIISAppVersion(id, newPath);
            return true;
        }
        


        /// <summary>
        /// 获取IIS应用程序信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private AppInfo GetIISAppInfo(long id)
        {
            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                Site site = iisManager.Sites.FirstOrDefault(f => f.Id == id);
                if (site == null)
                {
                    throw new ArgumentException($"未找到{id}的网站");
                }
                AppInfo app = new AppInfo
                {
                    AppId = site.Id,
                    AppName = site.Name,
                    AppPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                    Status = site.State == ObjectState.Started ? 1 : 0
                };
                return app;
            }
        }


      


        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="stream">待解压文件流</param>
        /// <param name="dir"> 解压到哪个目录中(包含物理路径)</param>
        private void UnpackFiles(Stream stream, string dir)
        {
            if (dir.EndsWith("\\") == false)
            {
                dir += "\\";
            }
            CreatePath(dir);
            using (ZipInputStream zipStream = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != string.Empty)
                    {
                        Directory.CreateDirectory(dir + directoryName);
                    }

                    if (fileName == string.Empty)
                    {
                        continue;
                    }

                    using (FileStream streamWriter = File.Create(dir + theEntry.Name))
                    {
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            int size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            };
        }


        private void ChangeIISAppVersion(long id, string dir)
        {
            using (ServerManager iisManager = new ServerManager(_applicationHostPath))
            {
                Site site = iisManager.Sites.FirstOrDefault(f => f.Id == id);
                if (site == null)
                {
                    throw new ArgumentException($"未找到{id}的网站");
                }
                site.Applications["/"].VirtualDirectories["/"].PhysicalPath = dir;

                iisManager.CommitChanges();

                ApplicationPool appPool = iisManager.ApplicationPools[site.Name];
                if (appPool == null)
                {
                    throw new Exception($"{site.Name}应用程序池不存在");
                }

                if (appPool.State != ObjectState.Started)
                {
                    appPool.Start();
                }
                else
                {
                    appPool.Recycle();
                }
                iisManager.CommitChanges();
            }
        }


        private string GetNewPath(long appId)
        {
            AppInfo app = GetIISAppInfo(appId);
            string[] arr = app.AppPath.Split(".");
            int number = Convert.ToInt32(arr[arr.Length - 1]);
            number += 1;
            arr[arr.Length - 1] = number.ToString();
            string newPath = string.Join(".", arr);
            return newPath;
        }



        private void CreatePath(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }
            Directory.CreateDirectory(path);
        }
    }
}
