using System.Reflection;
using Wjire.Excel.Model;

namespace Wjire.Excel
{

    /// <summary>
    /// 列信息
    /// </summary>
    internal class ColumnInfo
    {
        internal PropertyInfo PropertyInfo { get; set; }

        internal string DisplayName { get; set; }

        internal CellSettingAttribute CellSetting { get; set; }
    }
}
