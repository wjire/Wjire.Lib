using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace Wjire.ProjectManager.WebApi.Service
{
    public class FileService
    {

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="stream">待解压文件流</param>
        /// <param name="dir"> 解压到哪个目录中(包含物理路径)</param>
        public bool UnpackFiles(Stream stream, string dir)
        {
            ZipInputStream zipStream = null;
            try
            {
                CreateDir(dir);
                zipStream = new ZipInputStream(stream);
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

                    FileStream streamWriter = System.IO.File.Create(dir + theEntry.Name);
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = zipStream.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                zipStream?.Dispose();
            }
        }

        private void CreateDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                return;
            }
            Directory.CreateDirectory(dir);
        }
    }
}
