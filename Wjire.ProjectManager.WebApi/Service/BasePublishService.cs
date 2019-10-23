using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Utils;

namespace Wjire.ProjectManager.WebApi.Service
{
    public abstract class BasePublishService
    {

        protected readonly AppInfo AppInfo;

        protected BasePublishService()
        {

        }

        protected BasePublishService(AppInfo appInfo)
        {
            AppInfo = appInfo;
        }



        /// <summary>
        /// 获取所有程序信息
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<AppInfo> GetAppInfos();



        /// <summary>
        /// 发布程序
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual bool PublishApp(Stream stream)
        {
            bool isCover = Convert.ToBoolean(ConfigurationHelper.GetConfiguration("isCover"));
            if (isCover == false)
            {
                return PublishWithNew(stream);
            }

            StopApp();
            return PublishWithCover(stream);
        }



        /// <summary>
        /// 覆盖发布
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool PublishWithCover(Stream stream)
        {
            string path = GetCurrentPath();
            UnpackFiles(stream, path);
            CoverCurrentVersion(path);
            return true;
        }


        /// <summary>
        /// 新建版本发布
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool PublishWithNew(Stream stream)
        {
            string path = GetNewPath();
            UnpackFiles(stream, path);
            CreateNewVersion(path);
            return true;
        }

        protected abstract void StopApp();
        protected abstract string GetNewPath();
        protected abstract string GetCurrentPath();
        protected abstract void CreateNewVersion(string newPath);
        protected abstract void CoverCurrentVersion(string path);



        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="stream">待解压文件流</param>
        /// <param name="newPath"> 解压到哪个目录中(包含物理路径)</param>
        protected virtual void UnpackFiles(Stream stream, string newPath)
        {
            if (newPath.EndsWith("\\") == false)
            {
                newPath += "\\";
            }
            CreatePath(newPath);
            using (ZipInputStream zipStream = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != string.Empty)
                    {
                        Directory.CreateDirectory(newPath + directoryName);
                    }

                    if (fileName == string.Empty)
                    {
                        continue;
                    }

                    using (FileStream streamWriter = File.Create(newPath + theEntry.Name))
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
