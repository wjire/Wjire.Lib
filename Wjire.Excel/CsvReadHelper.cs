using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Wjire.Excel
{
    public class CsvReadHelper
    {

        public static List<T> ReadByDefaultEncoding<T>(string fileName, ClassMap classMap = null)
        {
            Encoding encoding = Encoding.Default;
            return Read<T>(fileName, encoding, classMap);
        }



        public static List<T> ReadByUTF8Encoding<T>(string fileName, ClassMap classMap = null)
        {
            Encoding encoding = Encoding.UTF8;
            return Read<T>(fileName, encoding, classMap);
        }


        public static List<T> ReadByGB2312<T>(string fileName, ClassMap classMap = null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            return Read<T>(fileName, encoding, classMap);
        }


        private static List<T> Read<T>(string fileName, Encoding encoding, ClassMap classMap)
        {
            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                CsvReader csv = new CsvReader(sr);
                if (classMap != null)
                {
                    csv.Configuration.RegisterClassMap(classMap);
                }
                csv.Configuration.BadDataFound = null;
                List<T> records = csv.GetRecords<T>().ToList();
                return records;
            }
        }
    }
}
