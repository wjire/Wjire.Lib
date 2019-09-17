using System;
using System.IO;
using System.Text;

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
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

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
        /// <param name="buffers">字节</param>
        /// <param name="isCover">是否覆盖,默认覆盖</param>
        public static void WriteByte(string path, byte[] buffers, bool isCover = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (buffers == null || buffers.Length == 0)
            {
                throw new ArgumentNullException(nameof(buffers));
            }

            FileMode mode = isCover ? FileMode.Create : FileMode.Append;
            using (FileStream fs = new FileStream(path, mode, FileAccess.Write))
            {
                fs.Write(buffers, 0, buffers.Length);
            }
        }
    }
}
