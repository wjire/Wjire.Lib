using System.Collections.Generic;
using System.IO;

namespace Wjire.Excel
{
    internal interface IWriteHandler
    {
        MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources);

        MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, ICollection<string> exportFields);

        MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName);

        byte[] CreateBytes<T>(IEnumerable<T> sources);

        byte[] CreateBytes<T>(IEnumerable<T> sources, ICollection<string> exportFields);

        byte[] CreateBytes<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName);

        void CreateFile<T>(IEnumerable<T> sources, string path);

        void CreateFile<T>(IEnumerable<T> sources, ICollection<string> exportFields, string path);

        void CreateFile<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName, string path);
    }
}
