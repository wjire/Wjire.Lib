using System.Collections.Generic;
using System.IO;

namespace Wjire.Excel.Interface
{
    internal interface IExcelHandler
    {

        MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, HashSet<string> exportFields);

        byte[] CreateBytes<T>(IEnumerable<T> sources, HashSet<string> exportFields);

        void CreateFile<T>(IEnumerable<T> sources, HashSet<string> exportFields, string path);
    }
}
