using System;
using System.IO;

namespace Wjire.Excel
{
    public static class ReadHandlerFactory
    {
        public static IReadHandler CreateHandler(string filePath)
        {
            string ext = Path.GetExtension(filePath);
            switch (ext)
            {
                case ".xlsx":
                    return new Excel2007ReadHandler(filePath);
                case ".xls":
                    return new Excel2003ReadHandler(filePath);
                default:
                    throw new ArgumentException($"不支持后缀名为 {ext} 的文件");
            }
        }
    }
}
