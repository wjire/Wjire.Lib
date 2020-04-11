using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Wjire.Common
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public static class FileHelper
    {

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="contents">写入的内容</param>
        /// <param name="encoding">编码格式,默认UTF8</param>
        public static void WriteString(string path, string contents, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            File.WriteAllText(path, contents, encoding);
        }


        /// <summary>
        /// 写入字节
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="bytes">字节</param>
        /// <param name="isCover">是否覆盖,默认覆盖</param>
        public static async Task WriteBytes(string path, byte[] bytes, bool isCover = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            FileMode mode = isCover ? FileMode.OpenOrCreate : FileMode.Append;
            using (FileStream fs = new FileStream(path, mode, FileAccess.Write, FileShare.Write))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }


        private const int BufferSize = 84975;//写出缓冲区大小

        /// <summary>
        /// 写文件导到磁盘
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <returns></returns>
        public static async Task<int> WriteFileAsync(string path, Stream stream, int bufferSize = BufferSize)
        {
            int writeCount = 0;
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, bufferSize, true))
            {
                byte[] byteArr = new byte[BufferSize];
                int readCount = 0;
                while ((readCount = await stream.ReadAsync(byteArr, 0, byteArr.Length)) > 0)
                {
                    await fileStream.WriteAsync(byteArr, 0, readCount);
                    writeCount += readCount;
                }
            }
            return writeCount;
        }
    }
}
