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
    public class ExePublishService
    {

        private readonly string _rpcServicePath;

        public ExePublishService(IConfiguration configuration)
        {
            _rpcServicePath = configuration["rpcServicePath"];
        }


        /// <summary>
        /// 获取所有rpc服务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppInfo> GetAllRpcService()
        {
            List<AppInfo> result = new List<AppInfo>();
            var dirs = Directory.GetDirectories(_rpcServicePath);
            return result;
        }


        /// <summary>
        /// 发布网站
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Publish(string appName, Stream stream)
        {
            string newPath = GetNewPath(appName);
            UnpackFiles(stream, newPath);
            ChangeAppVersion(newPath);
            return true;
        }


        private string GetNewPath(string appName)
        {
            var path = Path.Combine(_rpcServicePath, appName);
            var dirs = Directory.GetDirectories(path);

            var versions = new List<Version>();
            foreach (var item in dirs)
            {
                var versionString = Path.GetFileName(item);
                versions.Add(new Version(versionString));
            }

            var max = versions.Max();
            var arr = max.ToString().Split(".");
            var lastNumber = Convert.ToInt32(arr[arr.Length - 1]);
            arr[arr.Length - 1] = (++lastNumber).ToString();
            var newVersionString = string.Join(".", arr);
            return Path.Combine(path, newVersionString);
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


        private void ChangeAppVersion(string path)
        {

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
