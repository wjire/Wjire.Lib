using System.Reflection;

namespace Wjire.Excel
{

    /// <summary>
    /// 列信息
    /// </summary>
    internal class ColumnInfo
    {
        internal PropertyInfo PropertyInfo { get; set; }

        internal string DisplayName { get; set; }

        internal int Order { get; set; }
    }
}
