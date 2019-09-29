using System;
using System.IO;

namespace Wjire.CodeBuilder.Utils
{
    public class FileHelper
    {

        public static string ChangeFirstCharToUpper(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName.Trim()) == false)
            {
                return tableName.Substring(0, 1).ToUpper() + tableName.Substring(1);
            }
            return tableName;
        }


        public static void CheckDirectory(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                throw new ArgumentNullException(nameof(dir));
            }

            if (Directory.Exists(dir))
            {
                return;
            }

            Directory.CreateDirectory(dir);
        }


        public static void CreateFile(string path, string content, bool isCover = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string dir = Path.GetDirectoryName(path);
            CheckDirectory(dir);
            if (File.Exists(path) == false)
            {
                File.WriteAllText(path, content);
            }
            else if (isCover)
            {
                File.WriteAllText(path, content);
            }
        }
    }
}
