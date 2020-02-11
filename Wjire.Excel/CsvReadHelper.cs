using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace Wjire.Excel
{
    public class CsvReadHelper
    {

        public static List<T> ReadByDefaultEncoding<T>(string fileName)
        {
            Encoding encoding = Encoding.Default;
            return Read<T>(fileName, encoding);
        }



        public static List<T> ReadByUTF8Encoding<T>(string fileName)
        {
            Encoding encoding = Encoding.UTF8;
            return Read<T>(fileName, encoding);
        }


        public static List<T> ReadByGB2312<T>(string fileName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            return Read<T>(fileName, encoding);
        }


        private static List<T> Read<T>(string fileName, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                CsvReader csv = new CsvReader(sr);
                List<T> records = csv.GetRecords<T>().ToList();
                return records;
            }
        }
    }
}
