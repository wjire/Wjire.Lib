using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using Wjire.ProjectManager.WebApi.Model;

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
            string newPath = GetNewPath();
            UnpackFiles(stream, newPath);
            ChangeAppVersion(newPath);
            return true;
        }

        protected abstract string GetNewPath();


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

        protected abstract void ChangeAppVersion(string newPath);


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
